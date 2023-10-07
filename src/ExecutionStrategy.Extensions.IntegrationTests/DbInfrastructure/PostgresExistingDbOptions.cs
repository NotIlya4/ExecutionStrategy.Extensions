using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public record PostgresExistingDbOptions
{
    public required Func<AppDbContext, Task> OnBootstrap { get; set; }
    public required Func<AppDbContext, Task> OnClean { get; set; }
    public required Func<AppDbContext, Task> OnDestroy { get; set; }
    public int Port { get; set; } = 5432;
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "pgpass";
    public string Database { get; set; } = "Dev";
}