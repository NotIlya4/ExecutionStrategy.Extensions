using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public static class VoidExtensions
{
    public static Task ExecuteExtendedAsync<TDbContext>(this TDbContext context,
        ExecutionStrategyVoidNext<TDbContext> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, Void>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(async args =>
        {
            await operation(args);
            return new Void();
        }, action);
    }
    
    public static Task ExecuteExtendedAsync<TDbContext>(this TDbContext context,
        ExecutionStrategyVoidNext operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, Void>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(_ => operation(), action);
    }
    
    public static IExecutionStrategyOptionsBuilder<TDbContext, Void> WithOperation<TDbContext>(
        this IExecutionStrategyOptionsBuilder<TDbContext, Void> builder, ExecutionStrategyVoidNext<TDbContext> operation) where TDbContext : DbContext
    {
        return builder.WithOperation(async args =>
        {
            await operation(args);
            return new Void();
        });
    }
    
    public static IExecutionStrategyOptionsBuilder<TDbContext, Void> WithOperation<TDbContext>(
        this IExecutionStrategyOptionsBuilder<TDbContext, Void> builder, ExecutionStrategyVoidNext operation) where TDbContext : DbContext
    {
        return builder.WithOperation(_ => operation());
    }
    
    public static IExecutionStrategyOptionsBuilder<TDbContext, Void> WithVerifySucceeded<TDbContext>(
        this IExecutionStrategyOptionsBuilder<TDbContext, Void> builder, ExecutionStrategyNext<TDbContext, bool> verifySucceeded) where TDbContext : DbContext
    {
        return builder.WithVerifySucceeded(async args => new ExecutionResult<Void>(await verifySucceeded(args), new Void()));
    }

    public static IBuilderWithMiddleware<TDbContext, Void, TReturn> WithMiddleware<TDbContext, TReturn>(
        this IBuilderWithMiddleware<TDbContext, Void, TReturn> builder,
        ExecutionStrategyVoidMiddleware<TDbContext> middleware) where TReturn : IBuilderWithMiddleware<TDbContext, Void, TReturn> where TDbContext : DbContext
    {
        return builder.WithMiddleware(async (next, args) =>
        {
            await middleware(async (args) => await next(args), args);
            return new Void();
        });
    }
}