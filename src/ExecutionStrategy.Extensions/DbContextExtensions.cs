using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public static class DbContextExtensions
{
    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(this TDbContext context,
        ExecutionStrategyOperation<TDbContext, Task<TResult>> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, TResult>>? action = null) where TDbContext : DbContext
    {
        var options = context.CreateOptionsFromPrimary(operation);
        var builder = new ExecutionStrategyOptionsBuilder<TDbContext, TResult>(options);
        
        action?.Invoke(builder);

        return context.ExecuteExtendedAsync(options);
    }
    
    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(this TDbContext context,
        ExecutionStrategyOperation<Task<TResult>> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, TResult>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(args => operation(), action);
    }
}