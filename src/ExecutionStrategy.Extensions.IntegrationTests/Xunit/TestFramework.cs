﻿using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ExecutionStrategy.Extensions.IntegrationTests.Xunit;

class TestFramework : XunitTestFramework
{
    public TestFramework(IMessageSink messageSink)
        : base(messageSink)
    { }

    protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
        => new TestFrameworkExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
}
