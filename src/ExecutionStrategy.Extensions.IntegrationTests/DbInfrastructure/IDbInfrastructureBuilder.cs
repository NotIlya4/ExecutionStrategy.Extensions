using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public interface IDbInfrastructureBuilder<TDbContext> where TDbContext : DbContext
{
    IDbInfrastructure UsePostgresExistingDb(PostgresConn postgresConn);
    IDbInfrastructure UsePostgresLocalContainer(PostgresConn postgresConn);
}

public class DbInfrastructureBuilder<TDbContext> : IDbInfrastructureBuilder<TDbContext> where TDbContext : DbContext
{
    public IDbInfrastructure UsePostgresExistingDb(PostgresConn? postgresConn = null)
    {
        return new PostgresIsolatedInfrastructure<TDbContext>(postgresConn ?? new PostgresConn());
    }

    public IDbInfrastructure UsePostgresLocalContainer(PostgresConn? postgresConn = null)
    {
        return new PostgresLocalContainerInfrastructure<TDbContext>(postgresConn ?? new PostgresConn());
    }
}