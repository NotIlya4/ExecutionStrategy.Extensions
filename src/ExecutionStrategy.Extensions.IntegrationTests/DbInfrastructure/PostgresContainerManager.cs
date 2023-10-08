using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public class PostgresContainerManager
{
    private readonly PostgresContainerOptions _options;
    private IContainerService? _container;

    public PostgresContainerManager(PostgresContainerOptions options)
    {
        _options = options;
    }
    
    public void EnsureStarted()
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

    public void Recreate()
    {
        Destroy();
        EnsureStarted();
    }

    public void Destroy()
    {
        if (_container is null)
        {
            return;
        }
        
        _container.Dispose();
    }
}