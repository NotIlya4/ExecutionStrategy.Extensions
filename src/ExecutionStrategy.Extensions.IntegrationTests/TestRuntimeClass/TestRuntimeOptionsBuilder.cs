namespace ExecutionStrategy.Extensions.IntegrationTests.TestRuntimeClass;

public class TestRuntimeOptionsBuilder
{
    public DbBootstrapperBuilderPart DbBootstrapper { get; }

    public TestRuntimeOptionsBuilder()
    {
        DbBootstrapper = new DbBootstrapperBuilderPart(this);
    }
}