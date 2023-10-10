using System.Data;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public interface IBuilderWithMiddlewares<TDbContext, TResult, out TReturn> 
    where TDbContext : DbContext where TReturn : IBuilderWithMiddlewares<TDbContext, TResult, TReturn>
{
    TReturn WithMiddleware(ExecutionStrategyMiddleware<TDbContext, TResult> middleware);
    
}

public static class BuilderWithMiddlewareExtensions
{
    public static TReturn WithMiddleware<TDbContext, TReturn>(this IBuilderWithMiddlewares<TDbContext, Void, TReturn> builder, 
        ExecutionStrategyVoidMiddleware<TDbContext> middleware) 
        where TReturn : IBuilderWithMiddlewares<TDbContext, Void, TReturn> where TDbContext : DbContext
    {
        return builder.WithMiddleware(async (next, args) =>
        {
            await middleware(async (args) => await next(args), args);
            return Void.Instance;
        });
    }
    
    public static TReturn WithTransaction<TDbContext, TResult, TReturn>(this IBuilderWithMiddlewares<TDbContext, TResult, TReturn> builder, IsolationLevel isolationLevel)
        where TReturn : IBuilderWithMiddlewares<TDbContext, TResult, TReturn> where TDbContext : DbContext
    {
        return builder.WithMiddleware(async (next, args) =>
        {
            await using var transaction = await args.Context.Database.BeginTransactionAsync(isolationLevel);
            args.Data.Set(transaction);
            var result = await next(args);
            await transaction.CommitAsync();
            return result;
        });
    }

    public static TReturn WithClearChangeTrackerOnRetry<TDbContext, TResult, TReturn>(this IBuilderWithMiddlewares<TDbContext, TResult, TReturn> returnTo)
        where TReturn : IBuilderWithMiddlewares<TDbContext, TResult, TReturn> where TDbContext : DbContext
    {
        return returnTo.WithMiddleware(async (next, args) =>
        {
            args.Context.ChangeTracker.Clear();
            return await next(args);
        });
    }
}