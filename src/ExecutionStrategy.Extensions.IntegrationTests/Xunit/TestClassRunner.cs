using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ExecutionStrategy.Extensions.IntegrationTests.Xunit;

public class TestClassRunner : XunitTestClassRunner
{
    private readonly IServiceProvider _serviceProvider;

    public TestClassRunner(IServiceProvider serviceProvider, ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink, IMessageBus messageBus, ITestCaseOrderer testCaseOrderer,
        ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource,
        IDictionary<Type, object> collectionFixtureMappings) : base(testClass, @class, testCases, diagnosticMessageSink,
        messageBus, testCaseOrderer, aggregator, cancellationTokenSource, collectionFixtureMappings)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task<RunSummary> RunTestMethodAsync(ITestMethod testMethod, IReflectionMethodInfo method, IEnumerable<IXunitTestCase> testCases,
        object[] constructorArguments)
    {
        return new TestMethodRunner(testMethod, Class, method, testCases, DiagnosticMessageSink, MessageBus, new ExceptionAggregator(Aggregator), CancellationTokenSource, constructorArguments).RunAsync();
    }

    protected override bool TryGetConstructorArgument(ConstructorInfo constructor, int index, ParameterInfo parameter,
        [UnscopedRef] out object? argumentValue)
    {
        var service = _serviceProvider.GetService(parameter.ParameterType);
        argumentValue = service;

        return argumentValue is not null;
    }
}