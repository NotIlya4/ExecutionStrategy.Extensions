using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public class PostgresContainer
{
    private readonly PostgresContainerOptions _options;
    private IContainerService? _container;

    public PostgresContainer(PostgresContainerOptions options)
    {
        _options = options;
    }
    
    public async Task EnsureStarted()
    {
        _container = new Builder()
            .UseContainer()
            .WithName(_options.ContainerName)
            .ReuseIfExists()
            .UseImage(_options.Image)
            .WithEnvironment($"POSTGRES_PASSWORD={_options.Password}")
            .ExposePort(_options.Port, 5432)
            .WaitForMessageInLog("database system is ready to accept connections", TimeSpan.FromSeconds(10))
            .Build().Start();
    }

    public async Task Recreate()
    {
        await Destroy();
        await EnsureStarted();
    }

    public async Task Destroy()
    {
        if (_container is null)
        {
            return;
        }
        
        _container.Dispose();
    }
}