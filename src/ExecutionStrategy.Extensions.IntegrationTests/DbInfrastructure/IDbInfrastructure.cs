using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public interface IIsolatedDbInfrastructure : IDisposable
{
    void ConfigureDbContext(DbContextOptionsBuilder builder);
    void Clear();
    void Destroy();
}


public interface IDbInfrastructure : IDisposable
{
    IIsolatedDbInfrastructure ProvideIsolatedInfrastructure();
    void Destroy();
}