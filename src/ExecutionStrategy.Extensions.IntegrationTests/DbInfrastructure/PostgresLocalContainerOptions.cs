using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public record PostgresLocalContainerOptions
{
    public required Action<PostgresContainer, AppDbContext> OnBootstrap { get; set; }
    public required Action<PostgresContainer, AppDbContext> OnClean { get; set; }
    public required Action<PostgresContainer, AppDbContext> OnDestroy { get; set; }
    public string ContainerName { get; set; } = "postgres-test";
    public int Port { get; set; } = 5432;
    public string Image { get; set; } = "postgres:latest";
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "pgpass";
    public string Database { get; set; } = "Dev";
}