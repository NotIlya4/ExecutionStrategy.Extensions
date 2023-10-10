using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.Internal;

internal static class DelegateExtensions 
{
    public static ExecutionStrategyNext<DbContext, object> ToGeneric<TDbContext, TResult>(
        this ExecutionStrategyNext<TDbContext, TResult> operation) where TDbContext : DbContext
    {
        return async args => (await operation(args.FromGeneric<TDbContext>()))!;
    }

    public static ExecutionStrategyNext<TDbContext, TResult> FromGeneric<TDbContext, TResult>(
        this ExecutionStrategyNext<DbContext, object> operation) where TDbContext : DbContext
    {
        return async args => (TResult)await operation(args);
    }

    public static ExecutionStrategyMiddleware<DbContext, object> ToGeneric<TDbContext, TResult>(
        this ExecutionStrategyMiddleware<TDbContext, TResult> middleware) where TDbContext : DbContext
    {
        return async (next, args) => (await middleware(next.FromGeneric<TDbContext, TResult>(), args.FromGeneric<TDbContext>()))!;
    }
    
    public static ExecutionStrategyMiddleware<TDbContext, TResult> FromGeneric<TDbContext, TResult>(
        this ExecutionStrategyMiddleware<DbContext, object> middleware) where TDbContext : DbContext
    {
        return async (next, args) => (TResult)(await middleware(next.ToGeneric(), args));
    }

    public static IExecutionStrategyOperationArgs<DbContext> ToGeneric<TDbContext>(
        this IExecutionStrategyOperationArgs<TDbContext> args) where TDbContext : DbContext
    {
        return args;
    }
    
    public static IExecutionStrategyOperationArgs<TDbContext> FromGeneric<TDbContext>(
        this IExecutionStrategyOperationArgs<DbContext> args) where TDbContext : DbContext
    {
        return new ExecutionStrategyOperationArgs<TDbContext>(args.Data, (TDbContext)args.Context, args.Attempt, args.CancellationToken);
    }
}