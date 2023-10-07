namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public interface IDbBootstrapper : IDisposable
{
    void Bootstrap();
    void Destroy();
    void Clear();
}

public class DelegateBootstrapper : IDbBootstrapper
{
    private readonly Action _bootstrap;
    private readonly Action _clean;
    private readonly Action _destroy;

    public DelegateBootstrapper(Action bootstrap, Action clean, Action destroy)
    {
        _bootstrap = bootstrap;
        _clean = clean;
        _destroy = destroy;
    }
    
    public void Dispose()
    {
        Destroy();
    }

    public void Bootstrap()
    {
        _bootstrap();
    }

    public void Destroy()
    {
        _destroy();
    }

    public void Clear()
    {
        _clean();
    }
}