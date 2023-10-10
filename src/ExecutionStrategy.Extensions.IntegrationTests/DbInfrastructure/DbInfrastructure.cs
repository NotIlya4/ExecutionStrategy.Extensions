using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using ExecutionStrategy.Extensions.IntegrationTests.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

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

public static class NpgsqlConnExtensions
{
    public static NpgsqlConnectionStringBuilder ApplyPostgresConn(this NpgsqlConnectionStringBuilder builder, PostgresConn postgresConn)
    {
        builder["Host"] = postgresConn.Host;
        builder["Port"] = postgresConn.Port;
        builder["Username"] = postgresConn.Username;
        builder["Password"] = postgresConn.Password;
        builder["Database"] = postgresConn.Database;
        builder["Application Name"] = postgresConn.ApplicationName;
        return builder;
    }
}

public class InMemoryInfrastructure : IDbInfrastructure, IIsolatedDbInfrastructure
{
    private readonly DbContext _context;
    
    public InMemoryInfrastructure()
    {
        var builder = new DbContextOptionsBuilder();
        ConfigureDbContext(builder);
        _context = new DbContext(builder.Options);
        
        Clear();
    }
    
    public IIsolatedDbInfrastructure ProvideIsolatedInfrastructure()
    {
        return new InMemoryInfrastructure();
    }

    public void ConfigureDbContext(DbContextOptionsBuilder builder)
    {
        builder.UseInMemoryDatabase("default");
    }

    public void Clear()
    {
        _context.EnsureDeletedCreated();
    }

    public void Destroy()
    {
        _context.Database.EnsureDeleted();
    }

    public void Dispose()
    {
        Destroy();
    }
}

public interface IDbInfrastructureBuilder<TDbContext> where TDbContext : DbContext
{
    IDbInfrastructure UsePostgresExistingDb(PostgresConn postgresConn);
    IDbInfrastructure UsePostgresLocalContainer(PostgresConn postgresConn);
    IDbInfrastructure UseInMemory();
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
    
    public IDbInfrastructure UseInMemory()
    {
        return new InMemoryInfrastructure();
    }
}

public class DbInfrastructureProvider : IFixtureServicesProvider
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton((_) => new DbInfrastructureBuilder<AppDbContext>().UsePostgresLocalContainer());
    }
}