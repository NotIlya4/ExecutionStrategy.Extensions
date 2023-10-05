using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

public static class DbContextExtensions
{
    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TState, TResult>(
        this TDbContext context,
        Func<TDbContext, TState?, CancellationToken, Task<TResult>> operation,
        Action<ExecutionStrategyRuntimeOptions<TDbContext, TState?, TResult>>? optionsBuilder = null) where TDbContext : DbContext
    {
        var strategy = context.Database.CreateExecutionStrategy();

        return strategy.ExecuteExtendedAsync(operation, optionsBuilder);
    }
    
    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(
        this TDbContext context,
        Func<TDbContext, CancellationToken, Task<TResult>> operation,
        Action<ExecutionStrategyRuntimeOptions<TDbContext, bool, TResult>>? optionsBuilder = null) where TDbContext : DbContext
    {
        var strategy = context.Database.CreateExecutionStrategy();

        return strategy.ExecuteExtendedAsync(operation, optionsBuilder);
    }
}