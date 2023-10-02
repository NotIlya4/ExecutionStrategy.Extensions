using System.Data;
using EntityFrameworkCore.ExecutionStrategyExtended.Options;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public class ExecutionStrategyExtended<TDbContext> : IExecutionStrategyExtended<TDbContext>
    where TDbContext : DbContext
{
    private readonly ExecutionStrategyExtendedOptions<TDbContext> _options;
    public ExecutionStrategyExtendedData Data { get; }
    private IDbContextRetryBehaviorFactory<TDbContext> RetryBehaviorFactory => _options.DbContextRetryBehaviorFactory;
    private ActualDbContextProvider<TDbContext> ActualDbContextProvider => _options.ActualDbContextProvider;

    public ExecutionStrategyExtended(ExecutionStrategyExtendedOptions<TDbContext> options)
    {
        _options = options;
        Data = options.Data;
    }

    public async Task<TResponse> ExecuteAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext)
    {
        ActualDbContextProvider.DbContext = mainContext;

        var retrier = RetryBehaviorFactory.Create(mainContext);
        var strategy = retrier.CreateExecutionStrategy();
        var retryNumber = 1;

        return await strategy.ExecuteAsync(
            async () =>
            {
                var context = await retrier.ProvideDbContextForRetry(retryNumber);
                ActualDbContextProvider.DbContext = context;

                retryNumber += 1;
                return await action(context);
            });
    }

    public async Task<TResponse> ExecuteInTransactionAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext, IsolationLevel isolationLevel)
    {
        return await ExecuteAsync(async context =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync(isolationLevel);

            var response = await action(context);

            await transaction.CommitAsync();

            return response;
        }, mainContext);
    }
}