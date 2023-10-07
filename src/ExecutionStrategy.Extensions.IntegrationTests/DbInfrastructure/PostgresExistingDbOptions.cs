using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public record PostgresExistingDbOptions
{
    public required Action<AppDbContext> OnBootstrap { get; set; }
    public required Action<AppDbContext> OnClean { get; set; }
    public required Action<AppDbContext> OnDestroy { get; set; }
    public int Port { get; set; } = 5432;
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "pgpass";
    public string Database { get; set; } = "Dev";
}