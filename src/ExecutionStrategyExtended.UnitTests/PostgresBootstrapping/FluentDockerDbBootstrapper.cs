using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.UnitTests;

public record PostgresContainerOptions
{
    public string Image { get; set; } = "postgres:latest";
    public string Password { get; set; } = "pgpass";
    public int Port { get; set; } = 8888;
    public string ContainerName { get; set; } = "postgres-test";
}

public record FluentDockerOptions
{
    public required PostgresContainerOptions PostgresContainerOptions { get; set; }
    public required FluentDockerOnClean OnClean { get; set; }
}

public enum FluentDockerOnClean
{
    RecreateContainer,
    RecreateDb,
    CleanTables
}

public class FluentDockerDbBootstrapper : IDbBootstrapper
{
    private readonly FluentDockerOptions _options;
    private readonly DbContext _context;
    private IContainerService? _container;

    public FluentDockerDbBootstrapper(FluentDockerOptions options, DbContext context)
    {
        _options = options;
        _context = context;
    }
    
    public async Task Bootstrap()
    {
        await Destroy();
        
        _container = new Builder()
            .UseContainer()
            .WithName(_options.PostgresContainerOptions.ContainerName)
            .DeleteIfExists(true, true)
            .UseImage(_options.PostgresContainerOptions.Image)
            .WithEnvironment($"POSTGRES_PASSWORD={_options.PostgresContainerOptions.Password}")
            .ExposePort(_options.PostgresContainerOptions.Port, 5432)
            .WaitForMessageInLog("database system is ready to accept connections", TimeSpan.FromSeconds(10))
            .Build().Start();
    }

    public async Task Destroy()
    {
        if (_container is null)
        {
            return;
        }
        
        _container.Dispose();
    }

    public async Task Clean()
    {
        switch (_options.OnClean)
        {
            case FluentDockerOnClean.RecreateContainer:
                await Bootstrap();
                break;
            case FluentDockerOnClean.RecreateDb:
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.EnsureCreatedAsync();
                break;
            default:
                throw new ArgumentOutOfRangeException();
                break;
        }
    }
}