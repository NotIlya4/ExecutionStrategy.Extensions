using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

internal static class ExecutionStrategyDelegateExtensions 
{
    public static ExecutionStrategyOperation<DbContext, object> ToGeneric<TDbContext, TResult>(
        this ExecutionStrategyOperation<TDbContext, TResult> operation) where TDbContext : DbContext
    {
        return async args => (await operation(args.FromGeneric<TDbContext>()))!;
    }

    public static ExecutionStrategyOperation<TDbContext, TResult> FromGeneric<TDbContext, TResult>(
        this ExecutionStrategyOperation<DbContext, object> operation) where TDbContext : DbContext
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