using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public static class VoidExtensions
{
    public static Task ExecuteExtendedAsync<TDbContext>(this TDbContext context,
        ExecutionStrategyVoidOperation<TDbContext> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, ExecutionStrategyVoid>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(async args =>
        {
            await operation(args);
            return new ExecutionStrategyVoid();
        }, action);
    }
    
    public static Task ExecuteExtendedAsync<TDbContext>(this TDbContext context,
        ExecutionStrategyVoidOperation operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, ExecutionStrategyVoid>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(_ => operation(), action);
    }
    
    public static IExecutionStrategyOptionsBuilder<TDbContext, ExecutionStrategyVoid> WithOperation<TDbContext>(
        this IExecutionStrategyOptionsBuilder<TDbContext, ExecutionStrategyVoid> builder, ExecutionStrategyVoidOperation<TDbContext> operation) where TDbContext : DbContext
    {
        return builder.WithOperation(async args =>
        {
            await operation(args);
            return new ExecutionStrategyVoid();
        });
    }
    
    public static IExecutionStrategyOptionsBuilder<TDbContext, ExecutionStrategyVoid> WithOperation<TDbContext>(
        this IExecutionStrategyOptionsBuilder<TDbContext, ExecutionStrategyVoid> builder, ExecutionStrategyVoidOperation operation) where TDbContext : DbContext
    {
        return builder.WithOperation(_ => operation());
    }

    public static IBuilderWithMiddleware<TDbContext, ExecutionStrategyVoid, TReturn> WithMiddleware<TDbContext, TReturn>(
        this IBuilderWithMiddleware<TDbContext, ExecutionStrategyVoid, TReturn> builder,
        ExecutionStrategyVoidMiddleware<TDbContext> middleware) where TReturn : IBuilderWithMiddleware<TDbContext, ExecutionStrategyVoid, TReturn> where TDbContext : DbContext
    {
        return builder.WithMiddleware(async (next, args) =>
        {
            await middleware(async (args) => await next(args), args);
            return new ExecutionStrategyVoid();
        });
    }
}