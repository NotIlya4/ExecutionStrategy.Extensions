namespace ExecutionStrategy.Extensions.IntegrationTests.DbContextConfigurator;

public record PostgresConn
{
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "pgpass";
    public int Port { get; set; } = 5432;
    public string Server { get; set; } = "localhost";
}