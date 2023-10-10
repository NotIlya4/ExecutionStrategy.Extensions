using EntityFrameworkCore.ExecutionStrategy.Extensions.Internal;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public static class DbContextExtensions
{
    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(this TDbContext context,
        ExecutionStrategyNext<TDbContext, TResult> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, TResult>>? action = null) where TDbContext : DbContext
    {
        var options = context.CreateOptionsFromPrimary(operation);
        var builder = new ExecutionStrategyOptionsBuilder<TDbContext, TResult>(options);
        
        action?.Invoke(builder);

        return context.ExecuteExtendedAsync(options);
    }
    
    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(this TDbContext context,
        ExecutionStrategyNext<TResult> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, TResult>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(_ => operation(), action);
    }
    
    public static Task ExecuteExtendedAsync<TDbContext>(this TDbContext context,
        ExecutionStrategyVoidNext<TDbContext> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, Void>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(async args =>
        {
            await operation(args);
            return Void.Instance;
        }, action);
    }
    
    public static Task ExecuteExtendedAsync<TDbContext>(this TDbContext context,
        ExecutionStrategyVoidNext operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, Void>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(_ => operation(), action);
    }
}