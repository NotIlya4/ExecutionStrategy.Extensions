namespace EntityFrameworkCore.ExecutionStrategyExtended;

internal class IdempotencyTokenSaveResult
{
    public bool IsAlreadyExists { get; }

    public IdempotencyTokenSaveResult(bool isAlreadyExists)
    {
        IsAlreadyExists = isAlreadyExists;
    }
}