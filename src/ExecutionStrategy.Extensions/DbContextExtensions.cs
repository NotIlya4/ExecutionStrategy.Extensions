using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public static class DbContextExtensions
{
    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(this TDbContext context,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, TResult>> action) where TDbContext : DbContext
    {
        var primaryOptions = context.GetService<ExecutionStrategyOptions<TDbContext, TResult>>();
        var builder = new ExecutionStrategyOptionsBuilder<TDbContext, TResult>(primaryOptions);
        
        action(builder);

        return context.ExecuteExtendedAsync(primaryOptions);
    }

    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(this TDbContext context,
        ExecutionStrategyOperation<TDbContext, Task<TResult>> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, TResult>> action) where TDbContext : DbContext
    {
        Action<IExecutionStrategyOptionsBuilder<TDbContext, TResult>> newAction = builder =>
        {
            builder.WithOperation(operation);
            action(builder);
        };

        return context.ExecuteExtendedAsync(newAction);
    }
}