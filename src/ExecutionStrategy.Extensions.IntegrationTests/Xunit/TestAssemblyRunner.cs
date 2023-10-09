using System.Reflection;
using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ExecutionStrategy.Extensions.IntegrationTests.Xunit;

class TestAssemblyRunner : XunitTestAssemblyRunner
{
    public ServiceProvider ServiceProvider { get; set; }
    
    public TestAssemblyRunner(ITestAssembly testAssembly,
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink,
        IMessageSink executionMessageSink,
        ITestFrameworkExecutionOptions executionOptions)
        : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions)
    {
        GuardParallelism();
        UseProjectRelativeDirectory("VerifyGenerated");

        var services = new ServiceCollection();
        services.AddSingleton(_ => DbInfrastructureBuilder<AppDbContext>.Instance);
        ConfigureServiceFixtures(services);

        ServiceProvider = services.BuildServiceProvider();
    }

    protected override Task BeforeTestAssemblyFinishedAsync()
    {
        ServiceProvider.Dispose();

        return base.BeforeTestAssemblyFinishedAsync();
    }

    protected override async Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus,
        ITestCollection testCollection,
        IEnumerable<IXunitTestCase> testCases,
        CancellationTokenSource cancellationTokenSource)
    {
        return await new TestCollectionRunner(ServiceProvider, testCollection, testCases, DiagnosticMessageSink,
            messageBus, TestCaseOrderer, new ExceptionAggregator(Aggregator), cancellationTokenSource).RunAsync();
    }

    private void GuardParallelism()
    {
        if (typeof(TestFramework).Assembly.GetTypes().Any(x =>
                x.GetCustomAttributes().Any(x => x is CollectionAttribute || x is CollectionDefinitionAttribute)))
            throw new Exception(
                "Don't use CollectionAttribute and CollectionDefinitionAttribute because it decreases level of parallelism for tests");
    }

    private static void ConfigureServiceFixtures(IServiceCollection serviceCollection)
    {
        var types = typeof(TestFramework).Assembly.GetTypes();

        var fixtureLifetimes = new List<(Type, FixtureLifetimeAttribute)>();
        foreach (var type in types)
        {
            var attributes = type.GetCustomAttributes().OfType<FixtureLifetimeAttribute>().ToList();

            foreach (var attribute in attributes)
            {
                fixtureLifetimes.Add((type, attribute));
            }
        }

        foreach (var fixtureLifetime in fixtureLifetimes)
        {
            Type implementationType = fixtureLifetime.Item1;
            Type serviceType = fixtureLifetime.Item2.ServiceType ?? implementationType;
            ServiceLifetime lifetime = fixtureLifetime.Item2.Lifetime;
            serviceCollection.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
        }
    }
}