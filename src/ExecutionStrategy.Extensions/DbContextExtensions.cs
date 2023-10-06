using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public static class DbContextExtensions
{
    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(this TDbContext context,
        ExecutionStrategyOperation<TDbContext, Task<TResult>> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, TResult>>? action = null) where TDbContext : DbContext
    {
        var middlewares = context.GetMiddlewares<TDbContext, Task<TResult>>();
        var options = new ExecutionStrategyOptions<TDbContext, TResult>(
            new ExecutionStrategyData(),
            middlewares,
            operation,
            default,
            null);
        var builder = new ExecutionStrategyOptionsBuilder<TDbContext, TResult>(options);
        
        action?.Invoke(builder);

        return context.ExecuteExtendedAsync(options);
    }
}