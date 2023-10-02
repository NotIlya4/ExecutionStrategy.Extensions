using EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategyExtended.UnitTests;

public class TestFixture
{
    public IServiceProvider Services { get; }
    
    public TestFixture()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContextFactory<AppDbContext>(builder =>
        {
            builder.UseSqlite("Data Source=:memory:");
        });
        serviceCollection.AddExecutionStrategyExtended<AppDbContext>();

        Services = serviceCollection.BuildServiceProvider();
    }
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}

public interface IDbContextConfigurator
{
    public void ConfigureDbContext();
}

public interface IDbBootstrapper
{
    Task Bootstrap();
    Task Destroy();
    Task Clean();
}

public record PostgresConn
{
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "pgpass";
    public int Port { get; set; } = 5432;
    public string Server { get; set; } = "localhost";
}