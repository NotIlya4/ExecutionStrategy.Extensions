namespace EntityFrameworkCore.ExecutionStrategyExtended;

public interface IIdempotenceViolationDetector
{
    bool IsUniqueConstraintViolation(Exception e);
}