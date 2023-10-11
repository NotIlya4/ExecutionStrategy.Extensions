using Npgsql;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public static class NpgsqlConnExtensions
{
    public static NpgsqlConnectionStringBuilder ApplyPostgresConn(this NpgsqlConnectionStringBuilder builder, PostgresConn postgresConn)
    {
        builder["Host"] = postgresConn.Host;
        builder["Port"] = postgresConn.Port;
        builder["Username"] = postgresConn.Username;
        builder["Password"] = postgresConn.Password;
        builder["Database"] = postgresConn.Database;
        builder["Application Name"] = postgresConn.ApplicationName;
        return builder;
    }
}