using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

public class DefaultPostgresDetector : IIdempotenceViolationDetector
{
    private readonly IdempotencyTokenTableConfiguration _tableConfiguration;

    public DefaultPostgresDetector(IdempotencyTokenTableConfiguration tableConfiguration)
    {
        _tableConfiguration = tableConfiguration;
    }
    
    public bool IsUniqueConstraintViolation(Exception e)
    {
        if (e is DbUpdateException dbUpdateException)
        {
            if (dbUpdateException.InnerException is PostgresException postgresException)
            {
                return IsUniqueConstraintViolationInternal(postgresException);
            }
        }

        return false;
    }

    private bool IsUniqueConstraintViolationInternal(PostgresException postgresException)
    {
        return postgresException.ConstraintName == _tableConfiguration.PrimaryKeyConstraintName
               && postgresException.TableName == _tableConfiguration.TableName
               && postgresException.SqlState == PostgresErrorCodes.UniqueViolation;
    }
}