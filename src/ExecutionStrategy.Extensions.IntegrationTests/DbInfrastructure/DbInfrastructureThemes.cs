namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public static class DbInfrastructureThemes
{
    public static IDbInfrastructureBuilder CreateForLocalContainer()
    {
        return new DbInfrastructureBuilder()
            .UsePostgresLocalContainer(new PostgresLocalContainerOptions()
            {
                OnBootstrap = (container, context) => container.Recreate(),
                OnDestroy = (container, context) => container.Destroy(),
                OnClean = (container, context) => context.EnsureDeletedCreated(),
                Port = 8888
            });
    }

    public static IDbInfrastructureBuilder CreateForHelmRelease()
    {
        return new DbInfrastructureBuilder()
            .UsePostgresExistingDb(new PostgresExistingDbOptions()
            {
                OnBootstrap = context => context.ClearTables(),
                OnDestroy = context => context.ClearTables(),
                OnClean = context => context.ClearTables(),
                Port = 5432
            });
    }

    public static IDbInfrastructureBuilder CreateForSqlite()
    {
        return new DbInfrastructureBuilder().UseSqlite();
    }
}