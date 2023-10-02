namespace ExecutionStrategyExtended.UnitTests.PostgresBootstrapping.ExistingDb;

public record ExistingDbOptions
{
    public OnBootstrapType OnBootstrap { get; set; }
    public OnCleanType OnClean { get; set; }
    public OnDestroyType OnDestroy { get; set; }

    public enum OnBootstrapType
    {
        RecreateDb,
        CleanTables,
        DoNothing
    }

    public enum OnCleanType
    {
        RecreateDb,
        CleanTables,
        DoNothing
    }
    
    public enum OnDestroyType
    {
        DropDb,
        CleanTables,
        DoNothing,
    }
}