using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.UnitTests;

public class FluentDockerDbBootstrapper : IDbBootstrapper
{
    private readonly FluentDockerOptions _options;
    private readonly AppDbContext _context;
    private IContainerService? _container;

    public FluentDockerDbBootstrapper(FluentDockerOptions options, AppDbContext context)
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
            case FluentDockerOptions.OnCleanType.RecreateContainer:
                await Bootstrap();
                break;
            case FluentDockerOptions.OnCleanType.RecreateDb:
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.EnsureCreatedAsync();
                break;
            case FluentDockerOptions.OnCleanType.CleanTables:
                await _context.CleanTables();
                break;
            default:
                throw new ArgumentOutOfRangeException();
                break;
        }
    }

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await Destroy();
    }
}