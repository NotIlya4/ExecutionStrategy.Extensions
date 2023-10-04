using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

public static class ExecutionStrategyExtendedExtensions
{
    public static async Task<TResult> ExecuteExtendedAsync<TDbContext, TState, TResult>(
        this IExecutionStrategy strategy, 
        Func<TDbContext, TState?, CancellationToken, Task<TResult>> operation,
        ExecutionStrategyRuntimeOptions<TDbContext, TState?, TResult>? options = null) where TDbContext : DbContext
    {
        options ??= new ExecutionStrategyRuntimeOptions<TDbContext, TState?, TResult>();

        var retryContextProvider = options.DbContextForRetryProvider ?? (arguments => arguments.PreviousContext);

        var retryNumber = 1;
        TDbContext? previousContext = null;

        return await strategy.ExecuteAsync(options.State, async (actionContext, actionState, actionCancellationToken) =>
            {
                var mainContext = (TDbContext)actionContext;

                previousContext ??= mainContext;

                var retryContext = retryContextProvider(
                    new DbContextForRetryProviderArguments<TDbContext, TState?, TResult>(previousContext, mainContext, retryNumber, options));
                retryNumber += 1;

                return await operation(retryContext, actionState, actionCancellationToken);
            },
            options.VerifySucceeded is null
                ? null
                : async (actionContext, actionState, actionCancellationToken) =>
                    await options.VerifySucceeded((TDbContext)actionContext, actionState, actionCancellationToken),
            options.CancellationToken);
    }

    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TState, TResult>(
        this IExecutionStrategy strategy, 
        Func<TDbContext, TState?, CancellationToken, Task<TResult>> operation,
        Action<ExecutionStrategyRuntimeOptions<TDbContext, TState?, TResult>>? optionsBuilder = null)
        where TDbContext : DbContext
    {
        var options = new ExecutionStrategyRuntimeOptions<TDbContext, TState?, TResult>();
        optionsBuilder?.Invoke(options);

        return strategy.ExecuteExtendedAsync(operation, options);
    }

    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(
        this IExecutionStrategy strategy,
        Func<TDbContext, CancellationToken, Task<TResult>> operation,
        Action<ExecutionStrategyRuntimeOptions<TDbContext, bool, TResult>>? optionsBuilder = null)
        where TDbContext : DbContext
    {
        return strategy.ExecuteExtendedAsync(
            async (actionContext, _, actionCancellationToken) =>
                await operation(actionContext, actionCancellationToken), optionsBuilder);
    }

    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(
        this IExecutionStrategy strategy,
        Func<TDbContext, Task<TResult>> operation,
        Action<ExecutionStrategyRuntimeOptions<TDbContext, bool, TResult>>? optionsBuilder = null)
        where TDbContext : DbContext
    {
        return strategy.ExecuteExtendedAsync(async (actionContext, _) => await operation(actionContext),
            optionsBuilder);
    }

    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(
        this IExecutionStrategy strategy,
        Func<Task<TResult>> operation,
        Action<ExecutionStrategyRuntimeOptions<TDbContext, bool, TResult>>? optionsBuilder = null)
        where TDbContext : DbContext
    {
        return strategy.ExecuteExtendedAsync(async (_) => await operation(), optionsBuilder);
    }
}

public class ExecutionStrategyRuntimeOptions<TDbContext, TState, TResult> : Dictionary<object, object> where TDbContext : DbContext
{
    public Func<TDbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>>? VerifySucceeded { get; set; }

    public Func<DbContextForRetryProviderArguments<TDbContext, TState, TResult>, TDbContext>? DbContextForRetryProvider { get; set; }

    public TState? State { get; set; } = default;

    public CancellationToken CancellationToken { get; set; } = default;
}

public record DbContextForRetryProviderArguments<TDbContext, TState, TResult>(TDbContext PreviousContext,
    TDbContext MainContext, int Attempt, ExecutionStrategyRuntimeOptions<TDbContext, TState, TResult> Options)
    where TDbContext : DbContext
{
}