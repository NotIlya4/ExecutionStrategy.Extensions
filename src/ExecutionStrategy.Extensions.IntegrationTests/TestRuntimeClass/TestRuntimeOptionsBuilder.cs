namespace ExecutionStrategyExtended.UnitTests.TestRuntimeClass;

public class TestRuntimeOptionsBuilder
{
    public DbBootstrapperBuilderPart DbBootstrapper { get; }

    public TestRuntimeOptionsBuilder()
    {
        DbBootstrapper = new DbBootstrapperBuilderPart(this);
    }
}