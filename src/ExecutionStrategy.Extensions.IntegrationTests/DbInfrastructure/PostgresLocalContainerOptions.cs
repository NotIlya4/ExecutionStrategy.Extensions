using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public record PostgresLocalContainerOptions
{
    public required Func<PostgresContainer, AppDbContext, Task> OnBootstrap { get; set; }
    public required Func<PostgresContainer, AppDbContext, Task> OnClean { get; set; }
    public required Func<PostgresContainer, AppDbContext, Task> OnDestroy { get; set; }
    public string ContainerName { get; set; } = "postgres-test";
    public int Port { get; set; } = 5432;
    public string Image { get; set; } = "postgres:latest";
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "pgpass";
    public string Database { get; set; } = "Dev";
}