using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;

internal static class ExecutionStrategyMiddlewareDelegateExtensions 
{
    public static ExecutionStrategyOperation<TDbContext, TResult> CastOperation<TDbContext, TResult>(
        this ExecutionStrategyOperation<DbContext, object> generalOperation) where TDbContext : DbContext
    {
        return (args) => (TResult)generalOperation(args);
    }

    public static ExecutionStrategyOperation<DbContext, object> CastOperation<TDbContext, TResult>(
        this ExecutionStrategyOperation<TDbContext, TResult> operation) where TDbContext : DbContext
    {
        return args => operation(new ExecutionStrategyOperationArgs<TDbContext>(
            args.Data,
            (TDbContext)args.Context,
            args.Attempt,
            args.CancellationToken))!;
    }
    
    public static ExecutionStrategyMiddleware<TDbContext, TResult> CastMiddleware<TDbContext, TResult>(
        this ExecutionStrategyMiddleware<DbContext, object> generalMiddleware)
        where TDbContext : DbContext
    {
        return (next, args) => (TResult)CastOperation(next)(
            new ExecutionStrategyOperationArgs<TDbContext>(
                args.Data,
                args.Context,
                args.Attempt,
                args.CancellationToken));
    }

    public static ExecutionStrategyMiddleware<DbContext, object> CastMiddleware<TDbContext, TResult>(
        this ExecutionStrategyMiddleware<TDbContext, TResult> middleware)
        where TDbContext : DbContext
    {
        return (next, args) => CastOperation(next)(args);
    }
}