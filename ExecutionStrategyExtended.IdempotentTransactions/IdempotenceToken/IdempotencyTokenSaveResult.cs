namespace ExecutionStrategyExtended.IdempotentTransactions.IdempotenceToken;

internal class IdempotencyTokenSaveResult
{
    public bool IsAlreadyExists { get; }

    public IdempotencyTokenSaveResult(bool isAlreadyExists)
    {
        IsAlreadyExists = isAlreadyExists;
    }
}