namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public record PostgresConn
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5432;
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "pgpass";
    public string Database { get; set; } = "Dev";
    public string ApplicationName { get; set; } = "ExecutionStrategy.Extensions";
}