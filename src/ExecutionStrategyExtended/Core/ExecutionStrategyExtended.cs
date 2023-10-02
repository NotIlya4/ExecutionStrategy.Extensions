using System.Data;
using EntityFrameworkCore.ExecutionStrategyExtended.Configuration;
using EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public class ExecutionStrategyExtended<TDbContext> : IExecutionStrategyExtended<TDbContext>
    where TDbContext : DbContext
{
    private readonly IDbContextRetryBehaviorFactory<TDbContext> _retryBehaviorFactory;
    private readonly ActualDbContextProvider<TDbContext> _actualDbContextProvider;
    public ExecutionStrategyExtendedOptions<TDbContext> Options { get; }

    public ExecutionStrategyExtended(ExecutionStrategyExtendedOptions<TDbContext> options)
    {
        Options = options;
        _retryBehaviorFactory = options.DbContextRetryBehaviorFactory;
        _actualDbContextProvider = options.ActualDbContextProvider;
    }

    public async Task<TResponse> ExecuteAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext)
    {
        _actualDbContextProvider.DbContext = mainContext;

        var retrier = _retryBehaviorFactory.Create(mainContext);
        var strategy = retrier.CreateExecutionStrategy();
        var retryNumber = 1;

        return await strategy.ExecuteAsync(
            async () =>
            {
                var context = await retrier.ProvideDbContextForRetry(retryNumber);
                _actualDbContextProvider.DbContext = context;

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