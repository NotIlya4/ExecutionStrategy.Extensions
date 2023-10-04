using System.Data;
using EntityFrameworkCore.ExecutionStrategyExtended.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public class ExecutionStrategyExtended<TDbContext> : IExecutionStrategy where TDbContext : DbContext
{
    public IExecutionStrategy Strategy { get; set; }

    public ExecutionStrategyExtended(IExecutionStrategy strategy)
    {
        Strategy = strategy;
    }

    public TResult Execute<TState, TResult>(
        TState state, 
        Func<DbContext, TState, TResult> operation, 
        Func<DbContext, TState, ExecutionResult<TResult>>? verifySucceeded)
    {
        var retryNumber = 1;

        return Strategy.Execute(
            state,
            (actionContext, actionState) =>
            {
                var factory = actionContext.GetRetryBehavior();
                var retryBehavior = factory.Create((TDbContext)actionContext);
                
                var context = retryBehavior.ProvideDbContextForRetry(retryNumber).GetAwaiter().GetResult();
                retryNumber += 1;
                
                return operation(context, actionState);
            },
            verifySucceeded);
    }

    public async Task<TResult> ExecuteAsync<TState, TResult>(
        TState state,
        Func<DbContext, TState, CancellationToken, Task<TResult>> operation,
        Func<DbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>>? verifySucceeded,
        CancellationToken cancellationToken = default)
    {
        var retryNumber = 1;

        return await Strategy.ExecuteAsync(
            state,
            async (actionContext, actionState, actionCancellationToken) =>
            {
                var factory = actionContext.GetRetryBehavior();
                var retryBehavior = factory.Create((TDbContext)actionContext);
                
                var context = await retryBehavior.ProvideDbContextForRetry(retryNumber);
                retryNumber += 1;
                
                return await operation(context, actionState, actionCancellationToken);
            },
            verifySucceeded,
            cancellationToken);
    }

    public bool RetriesOnFailure { get; set; } = true;
}