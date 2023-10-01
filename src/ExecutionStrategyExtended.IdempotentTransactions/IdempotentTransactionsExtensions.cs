using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExecutionStrategyExtended.IdempotentTransactions;

public static class IdempotentTransactionsExtensions
{
    public static async Task<TResponse> ExecuteInIdempotentTransactionAsync<TDbContext, TResponse>(
        this IExecutionStrategyExtended<TDbContext> strategyExtended,
        Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext, string idempotencyToken, IsolationLevel isolationLevel) where TDbContext : DbContext
    {
        var factory = strategyExtended.Configuration.IdempotencyTransactionsMainFactory<TDbContext>();
        var token = factory.TokenManager.CreateIdempotencyToken(idempotencyToken);

        return await strategyExtended.ExecuteInTransactionAsync(async context =>
            {
                var service = factory.CreateService(context);
                return await service.HandleAction(async () => await action(context), token);
            },
            mainContext, isolationLevel);
    }
}