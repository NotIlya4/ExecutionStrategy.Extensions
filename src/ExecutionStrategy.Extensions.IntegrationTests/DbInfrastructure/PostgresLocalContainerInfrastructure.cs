using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public class PostgresLocalContainerInfrastructure<TDbContext> : IDbInfrastructure where TDbContext : DbContext
{
    private readonly PostgresConn _postgresConn;
    private readonly PostgresContainerManager _container;
    
    public PostgresLocalContainerInfrastructure(PostgresConn postgresConn)
    {
        _postgresConn = postgresConn;
        _container = new PostgresContainerManager(new PostgresContainerOptions()
        {
            Password = postgresConn.Password,
            Port = postgresConn.Port
        });
        _container.EnsureStarted();
    }

    public IIsolatedDbInfrastructure ProvideIsolatedInfrastructure()
    {
        return new PostgresIsolatedInfrastructure<TDbContext>(_postgresConn with
        {
            Database = Guid.NewGuid().ToString()
        });
    }

    public void Destroy()
    {
        _container.Destroy();
    }

    public void Dispose()
    {
        Destroy();
    }
}