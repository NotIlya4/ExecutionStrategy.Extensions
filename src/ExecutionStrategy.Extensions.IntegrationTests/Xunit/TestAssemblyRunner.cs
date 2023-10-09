using System.Reflection;
using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ExecutionStrategy.Extensions.IntegrationTests.Xunit;

class TestAssemblyRunner : XunitTestAssemblyRunner
{
    readonly Dictionary<Type, object> _assemblyFixtures = new();
    private readonly IServiceProvider _provider;

    public TestAssemblyRunner(ITestAssembly testAssembly,
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink,
        IMessageSink executionMessageSink,
        ITestFrameworkExecutionOptions executionOptions)
        : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions)
    {
        GuardParallelism();
        
        UseProjectRelativeDirectory("VerifyGenerated");
        
        var serviceCollection = new ServiceCollection();

        _provider = serviceCollection.BuildServiceProvider();

        _assemblyFixtures[typeof(IDbInfrastructure)] = DbInfrastructureBuilder<AppDbContext>.Instance;
    }

    protected override Task BeforeTestAssemblyFinishedAsync()
    {
        foreach (var disposable in _assemblyFixtures.Values.OfType<IDisposable>())
            Aggregator.Run(disposable.Dispose);

        return base.BeforeTestAssemblyFinishedAsync();
    }

    protected override async Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus,
        ITestCollection testCollection,
        IEnumerable<IXunitTestCase> testCases,
        CancellationTokenSource cancellationTokenSource)
    {
        return await new TestCollectionRunner(_assemblyFixtures, testCollection, testCases, DiagnosticMessageSink,
            messageBus,
            TestCaseOrderer, new ExceptionAggregator(Aggregator), cancellationTokenSource).RunAsync();
    }

    private void GuardParallelism()
    {
        if (typeof(TestFramework).Assembly.GetTypes().Any(x =>
                x.GetCustomAttributes().Any(x => x is CollectionAttribute || x is CollectionDefinitionAttribute)))
            throw new Exception(
                "Don't use CollectionAttribute and CollectionDefinitionAttribute because it decreases level of parallelism for tests");
    }
}