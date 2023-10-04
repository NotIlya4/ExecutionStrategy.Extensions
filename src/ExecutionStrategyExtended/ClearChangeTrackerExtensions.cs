using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using EntityFrameworkCore.ExecutionStrategyExtended.Core.DbContextRetryBehaviorImplementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

public static class ClearChangeTrackerExtensions
{
    public static Task<TResult> ExecuteWithChangeTrackerClearAsync<TDbContext, TState, TResult>(
        this IExecutionStrategy strategy,
        Func<TDbContext, TState?, CancellationToken, Task<TResult>> operation,
        Action<ExecutionStrategyRuntimeOptions<TDbContext, TState?, TResult>>? optionsBuilder = null)
        where TDbContext : DbContext
    {
        return strategy.ExecuteExtendedAsync(
            operation,
            runtimeOptions =>
            {
                runtimeOptions.DbContextForRetryProvider = arguments =>
                {
                    arguments.MainContext.ChangeTracker.Clear();
                    return arguments.MainContext;
                };
                optionsBuilder?.Invoke(runtimeOptions);
            });
    }

    public static Task<TResult> ExecuteWithChangeTrackerClearAsync<TDbContext, TResult>(
        this IExecutionStrategy strategy,
        Func<TDbContext, CancellationToken, Task<TResult>> operation,
        Action<ExecutionStrategyRuntimeOptions<TDbContext, bool, TResult>>? optionsBuilder = null)
        where TDbContext : DbContext
    {
        return strategy.ExecuteWithChangeTrackerClearAsync(
            async (c, _, ct) => await operation(c, ct),
            optionsBuilder);
    }
    
    public static Task<TResult> ExecuteWithChangeTrackerClearAsync<TDbContext, TResult>(
        this IExecutionStrategy strategy,
        Func<TDbContext, Task<TResult>> operation,
        Action<ExecutionStrategyRuntimeOptions<TDbContext, bool, TResult>>? optionsBuilder = null)
        where TDbContext : DbContext
    {
        return strategy.ExecuteWithChangeTrackerClearAsync(
            async (c, _) => await operation(c),
            optionsBuilder);
    }
    
    public static Task<TResult> ExecuteWithChangeTrackerClearAsync<TDbContext, TResult>(
        this IExecutionStrategy strategy,
        Func<Task<TResult>> operation,
        Action<ExecutionStrategyRuntimeOptions<TDbContext, bool, TResult>>? optionsBuilder = null)
        where TDbContext : DbContext
    {
        return strategy.ExecuteWithChangeTrackerClearAsync(
            async (_) => await operation(),
            optionsBuilder);
    }
}

public record ClearChangeTrackerRuntimeOptions
{
    
}