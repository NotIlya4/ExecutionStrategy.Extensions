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
        testCases = testCases.ToList();
        var testCasesPostgres = testCases.Select(t =>
        {
            var propertyInfo = t.GetType().GetProperties().Single(p => p.Name == nameof(IXunitTestCase.DisplayName));
            propertyInfo.SetValue(t, $"{t.DisplayName} - postgres");

            return t;
        }).ToList();
        
        var summary1 = await new TestCollectionRunner(_assemblyFixtures, testCollection, testCasesPostgres.ToList(), DiagnosticMessageSink, messageBus,
            TestCaseOrderer, new ExceptionAggregator(Aggregator), cancellationTokenSource).RunAsync();

        var testCasesSqlServer = testCases.Select(t =>
        {
            var propertyInfo = t.GetType().GetProperties().Single(p => p.Name == nameof(IXunitTestCase.DisplayName));
            propertyInfo.SetValue(t, $"{t.DisplayName} - sql server");

            return t;
        }).ToList();

        var summary2 = await new TestCollectionRunner(_assemblyFixtures, testCollection, testCasesSqlServer, DiagnosticMessageSink, messageBus,
            TestCaseOrderer, new ExceptionAggregator(Aggregator), cancellationTokenSource).RunAsync();

        summary1.Aggregate(summary2);
        
        return summary1;
    }
}
