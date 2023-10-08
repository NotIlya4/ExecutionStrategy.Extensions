using Xunit.Abstractions;
using Xunit.Sdk;

namespace ExecutionStrategy.Extensions.IntegrationTests.Xunit;

public class TestMethodRunner : XunitTestMethodRunner
{
    public TestMethodRunner(ITestMethod testMethod, IReflectionTypeInfo @class, IReflectionMethodInfo method, IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink, IMessageBus messageBus, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource, object[] constructorArguments) : base(testMethod, @class, method, testCases, diagnosticMessageSink, messageBus, aggregator, cancellationTokenSource, constructorArguments)
    {
    }
}