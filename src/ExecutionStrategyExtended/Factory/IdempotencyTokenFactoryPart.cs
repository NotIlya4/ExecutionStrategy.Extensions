using EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

internal class IdempotencyTokenFactoryPart
{
    private readonly IExecutionStrategyExtendedConfiguration _extendedConfiguration;

    public IdempotencyTokenFactoryPart(IExecutionStrategyExtendedConfiguration extendedConfiguration)
    {
        _extendedConfiguration = extendedConfiguration;
    }

    public IdempotencyTokenManager CreateManager()
    {
        return new IdempotencyTokenManager(_extendedConfiguration.SystemClock, _extendedConfiguration.ResponseSerializer);
    }

    public IdempotencyTokenRepository CreateRepository(DbContext context)
    {
        if (_extendedConfiguration.IdempotenceViolationDetector is null)
        {
            throw new InvalidOperationException($"You need to provide {nameof(IIdempotenceViolationDetector)} to work with idempotent transactions");
        }
        
        return new IdempotencyTokenRepository(context, _extendedConfiguration.IdempotenceViolationDetector);
    }

    public IdempotencyTokenService CreateService(DbContext context)
    {
        return new IdempotencyTokenService(CreateRepository(context), CreateManager());
    }
}