namespace ExecutionStrategy.Extensions.IntegrationTests.PostgresBootstrapping;

public interface IDbBootstrapper : IDisposable, IAsyncDisposable
{
    Task Bootstrap();
    Task Destroy();
    Task Clean();
}