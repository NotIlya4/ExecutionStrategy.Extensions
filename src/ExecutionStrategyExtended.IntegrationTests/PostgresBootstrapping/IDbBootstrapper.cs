namespace ExecutionStrategyExtended.UnitTests;

public interface IDbBootstrapper : IDisposable, IAsyncDisposable
{
    Task Bootstrap();
    Task Destroy();
    Task Clean();
}