﻿using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ExecutionStrategy.Extensions.IntegrationTests.Xunit;

public class TestClassRunner : XunitTestClassRunner
{
    public TestClassRunner(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink, IMessageBus messageBus, ITestCaseOrderer testCaseOrderer,
        ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource,
        IDictionary<Type, object> collectionFixtureMappings) : base(testClass, @class, testCases, diagnosticMessageSink,
        messageBus, testCaseOrderer, aggregator, cancellationTokenSource, collectionFixtureMappings)
    {
    }

    protected override Task AfterTestClassStartingAsync()
    {
        var fixtures = SelectTestClassConstructor()
            .GetParameters()
            .Select(parameter => parameter.ParameterType)
            .ToList();

        fixtures.ForEach(CreateClassFixture);

        
        return base.AfterTestClassStartingAsync();
    }

    protected override Task<RunSummary> RunTestMethodAsync(ITestMethod testMethod, IReflectionMethodInfo method, IEnumerable<IXunitTestCase> testCases,
        object[] constructorArguments)
    {
        return new TestMethodRunner(testMethod, Class, method, testCases, DiagnosticMessageSink, MessageBus, new ExceptionAggregator(Aggregator), CancellationTokenSource, constructorArguments).RunAsync();
    }
}