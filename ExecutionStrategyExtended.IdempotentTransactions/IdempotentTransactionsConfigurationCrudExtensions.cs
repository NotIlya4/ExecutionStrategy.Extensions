using ExecutionStrategyExtended.IdempotentTransactions.IdempotenceToken;
using ExecutionStrategyExtended.IdempotentTransactions.ViolationDetector;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.IdempotentTransactions;

internal static class IdempotentTransactionsConfigurationCrudExtensions
{
    internal static IdempotencyTokenMainFactory<TDbContext> IdempotencyTransactionsMainFactory<TDbContext>(
        this IExecutionStrategyExtendedConfiguration configuration) where TDbContext : DbContext
    {
        return (IdempotencyTokenMainFactory<TDbContext>)configuration.Data[nameof(IdempotencyTokenMainFactory<TDbContext>)];
    }
    
    internal static void IdempotencyTransactionsMainFactory<TDbContext>(
        this IExecutionStrategyExtendedConfiguration configuration, IdempotencyTokenMainFactory<TDbContext> factory) where TDbContext : DbContext
    {
        configuration.Data[nameof(IdempotencyTokenMainFactory<TDbContext>)] = factory;
    }
}

internal class IdempotencyTokenMainFactory<TDbContext> where TDbContext : DbContext
{
    private const string IdempotencyTokenManagerKey = nameof(IdempotencyTokenManager);
    private const string IdempotenceViolationDetectorKey = nameof(IIdempotenceViolationDetector);
    private const string ResponseSerializerKey = nameof(IResponseSerializer);
    private readonly IExecutionStrategyExtendedConfiguration _configuration;

    public IdempotencyTokenMainFactory(IExecutionStrategyExtendedConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IIdempotenceViolationDetector ViolationDetector =>
        (IIdempotenceViolationDetector)_configuration.Data[IdempotenceViolationDetectorKey];

    public IdempotencyTokenManager TokenManager =>
        (IdempotencyTokenManager)_configuration.Data[IdempotencyTokenManagerKey];

    public IResponseSerializer ResponseSerializer => (IResponseSerializer)_configuration.Data[ResponseSerializerKey];

    public IdempotencyTokenRepository CreateRepository(TDbContext context)
    {
        return new IdempotencyTokenRepository(context, ViolationDetector);
    }

    public IdempotencyTokenService CreateService(TDbContext context)
    {
        return new IdempotencyTokenService(CreateRepository(context), TokenManager);
    }
}