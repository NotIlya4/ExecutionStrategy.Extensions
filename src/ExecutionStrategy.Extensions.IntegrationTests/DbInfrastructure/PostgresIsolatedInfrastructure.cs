using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public class PostgresIsolatedInfrastructure<TDbContext> : IIsolatedDbInfrastructure, IDbInfrastructure where TDbContext : DbContext
{
    private readonly PostgresConn _postgresConn;
    private readonly DbContext _context;
    
    public PostgresIsolatedInfrastructure(PostgresConn postgresConn)
    {
        _postgresConn = postgresConn;

        var services = new ServiceCollection();
        services.AddDbContext<TDbContext>(ConfigureDbContext, ServiceLifetime.Transient);
        _context = services.BuildServiceProvider().GetRequiredService<TDbContext>();

        Clear();
    }
    
    public void ConfigureDbContext(DbContextOptionsBuilder builder)
    {
        builder.UseNpgsql(new NpgsqlConnectionStringBuilder().ApplyPostgresConn(_postgresConn).ConnectionString,
            optionsBuilder => optionsBuilder.EnableRetryOnFailure());
    }

    public void Clear()
    {
        _context.EnsureDeletedCreated();
    }

    public IIsolatedDbInfrastructure ProvideIsolatedInfrastructure()
    {
        return new PostgresIsolatedInfrastructure<TDbContext>(_postgresConn with {Database = Guid.NewGuid().ToString()});
    }

    public void Destroy()
    {
        _context.Database.EnsureDeleted();
    }

    public void Dispose()
    {
        Destroy();
        _context.Dispose();
    }
}