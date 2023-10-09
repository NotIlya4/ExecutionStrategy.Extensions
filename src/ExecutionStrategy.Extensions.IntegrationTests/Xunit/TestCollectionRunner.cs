using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ExecutionStrategy.Extensions.IntegrationTests.Xunit;

class TestCollectionRunner : XunitTestCollectionRunner
{
    private readonly Dictionary<Type, object> _assemblyFixtures;
    readonly IMessageSink _diagnosticMessageSink;

    public TestCollectionRunner(Dictionary<Type, object> assemblyFixtures,
        ITestCollection testCollection,
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        ITestCaseOrderer testCaseOrderer,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource)
        : base(testCollection, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource)
    {
        _assemblyFixtures = assemblyFixtures;
        _diagnosticMessageSink = diagnosticMessageSink;
    }

    protected override Task<RunSummary> RunTestClassAsync(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases)
    {
        var combinedFixtureMapping = CombineAssemblyFixtures();

        return new TestClassRunner(testClass, @class, testCases, DiagnosticMessageSink, MessageBus, TestCaseOrderer, new ExceptionAggregator(Aggregator), CancellationTokenSource, combinedFixtureMapping).RunAsync();
    }

    private Dictionary<Type, object> CombineAssemblyFixtures()
    {
        var combinedFixtureMapping = new Dictionary<Type, object>();

        foreach (KeyValuePair<Type,object> assemblyFixture in _assemblyFixtures)
        {
            combinedFixtureMapping[assemblyFixture.Key] = assemblyFixture.Value;
        }

        foreach (var fixtureMapping in CollectionFixtureMappings)
        {
            combinedFixtureMapping[fixtureMapping.Key] = fixtureMapping.Value;
        }

        return combinedFixtureMapping;
    }
}
