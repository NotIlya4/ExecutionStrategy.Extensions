namespace ExecutionStrategyExtended.UnitTests.PostgresBootstrapping;

public interface IDbBootstrapper : IDisposable, IAsyncDisposable
{
    Task Bootstrap();
    Task Destroy();
    Task Clean();
}