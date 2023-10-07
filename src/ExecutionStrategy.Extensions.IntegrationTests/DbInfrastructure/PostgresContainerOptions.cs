namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public record PostgresContainerOptions
{
    public string Image { get; set; } = "postgres:latest";
    public string Password { get; set; } = "pgpass";
    public int Port { get; set; } = 8888;
    public string ContainerName { get; set; } = "postgres-test";
}