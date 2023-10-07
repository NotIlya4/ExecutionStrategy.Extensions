namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public interface IDbBootstrapper : IDisposable, IAsyncDisposable
{
    Task Bootstrap();
    Task Destroy();
    Task Clean();
}

public class DelegateBootstrapper : IDbBootstrapper
{
    private readonly Func<Task> _bootstrap;
    private readonly Func<Task> _clean;
    private readonly Func<Task> _destroy;

    public DelegateBootstrapper(Func<Task> bootstrap, Func<Task> clean, Func<Task> destroy)
    {
        _bootstrap = bootstrap;
        _clean = clean;
        _destroy = destroy;
    }
    
    public void Dispose()
    {
        Destroy().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await Destroy();
    }

    public async Task Bootstrap()
    {
        await _bootstrap();
    }

    public async Task Destroy()
    {
        await _destroy();
    }

    public async Task Clean()
    {
        await _clean();
    }
}