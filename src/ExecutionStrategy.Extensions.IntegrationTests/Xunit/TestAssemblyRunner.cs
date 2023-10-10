using System.Reflection;
using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        UseProjectRelativeDirectory("VerifyGenerated");

        var services = new ServiceCollection();
        ConfigureFixtureServices(services);
        ConfigureFixtureServiceProviders(services);

        ServiceProvider = services.BuildServiceProvider();
        
        VerifyEfCore();
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

    private static void ConfigureFixtureServiceProviders(IServiceCollection serviceCollection)
    {
        var providers = typeof(TestFramework)
            .Assembly
            .GetTypes()
            .Where(t => !t.IsInterface && t.IsAssignableTo(typeof(IFixtureServicesProvider)))
            .Select((t) => (IFixtureServicesProvider)Activator.CreateInstance(t)!)
            .ToArray();

        foreach (var provider in providers)
        {
            provider.ConfigureServices(serviceCollection);
        }
    }

    private static void ConfigureFixtureServices(IServiceCollection serviceCollection)
    {
        var types = typeof(TestFramework).Assembly.GetTypes();

        var fixtureLifetimes = new List<(Type, FixtureServiceAttribute)>();
        foreach (var type in types)
        {
            var attributes = type.GetCustomAttributes().OfType<FixtureServiceAttribute>().ToList();

            foreach (var attribute in attributes)
            {
                fixtureLifetimes.Add((type, attribute));
            }
        }

        foreach (var fixtureLifetime in fixtureLifetimes)
        {
            Type implementationType = fixtureLifetime.Item1;
            Func<IServiceProvider, object>? factory = fixtureLifetime.Item2.Factory;
            Type serviceType = fixtureLifetime.Item2.ServiceType ?? implementationType;
            ServiceLifetime lifetime = fixtureLifetime.Item2.Lifetime;

            if (factory is null)
            {
                serviceCollection.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
            }
            else
            {
                serviceCollection.Add(new ServiceDescriptor(serviceType, factory, lifetime));
            }
        }
    }

    private void VerifyEfCore()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        var db = ServiceProvider.GetRequiredService<IDbInfrastructure>().ProvideIsolatedInfrastructure();
        db.ConfigureDbContext(builder);
        db.Dispose();

        VerifyEntityFramework.Initialize(new AppDbContext(builder.Options).Model);
    }
}