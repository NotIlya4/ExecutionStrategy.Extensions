using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ExecutionStrategy.Extensions.IntegrationTests.Xunit;

public class TestMethodRunner : XunitTestMethodRunner
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConstructorInfo _constructorInfo;
    private readonly IMessageSink _diagnosticMessageSink;
    private readonly object[] _constructorArguments;

    public TestMethodRunner(IServiceProvider serviceProvider, ConstructorInfo constructorInfo, ITestMethod testMethod,
        IReflectionTypeInfo @class, IReflectionMethodInfo method, IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink, IMessageBus messageBus, ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource, object[] constructorArguments) : base(testMethod, @class,
        method, testCases, diagnosticMessageSink, messageBus, aggregator, cancellationTokenSource, constructorArguments)
    {
        _serviceProvider = serviceProvider;
        _constructorInfo = constructorInfo;
        _diagnosticMessageSink = diagnosticMessageSink;

        var ctorParameters = constructorInfo.GetParameters().Select(p =>
        {
            var type = p.ParameterType;
            return serviceProvider.GetRequiredService(type);
        }).ToArray();

        _constructorArguments = ctorParameters;
    }

    protected override async Task<RunSummary> RunTestCaseAsync(IXunitTestCase testCase)
    {
        var fixtures = GetFixturesForHook();

        foreach (var fixture in fixtures)
        {
            await fixture.OnTestStart();
        }

        var usesVerifyAttribute = new UsesVerifyAttribute();

        usesVerifyAttribute.Before(TestMethod.Method.ToRuntimeMethod());

        var result = await testCase.RunAsync(_diagnosticMessageSink, MessageBus, _constructorArguments,
            new ExceptionAggregator(Aggregator), CancellationTokenSource);

        usesVerifyAttribute.After(TestMethod.Method.ToRuntimeMethod());

        foreach (var fixture in fixtures)
        {
            await fixture.OnTestFinish();
        }

        return result;
    }

    public List<ITestLifetime> GetFixturesForHook()
    {
        return _constructorArguments.Select(x => x as ITestLifetime).Where(x => x is not null).ToList()!;
    }
}