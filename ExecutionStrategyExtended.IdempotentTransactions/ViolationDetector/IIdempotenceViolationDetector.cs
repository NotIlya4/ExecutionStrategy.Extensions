namespace ExecutionStrategyExtended.IdempotentTransactions.ViolationDetector;

public interface IIdempotenceViolationDetector
{
    bool IsUniqueConstraintViolation(Exception e);
}