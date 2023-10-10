using System.Data;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public interface IBuilderWithMiddleware<TDbContext, TResult, out TReturn> where TDbContext : DbContext
{
    TReturn WithMiddleware(ExecutionStrategyMiddleware<TDbContext, TResult> middleware);
}

public static class BuilderWithMiddlewareExtensions
{
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
    
    public static TReturn WithTransaction<TDbContext, TResult, TReturn>(
        this IBuilderWithMiddleware<TDbContext, TResult, TReturn> returnTo, IsolationLevel isolationLevel)
        where TReturn : IBuilderWithMiddleware<TDbContext, TResult, TReturn> where TDbContext : DbContext
    {
        return returnTo.WithMiddleware(async (next, args) =>
        {
            await using var transaction = await args.Context.Database.BeginTransactionAsync(isolationLevel);
            args.Data.Set(transaction);
            var result = await next(args);
            await transaction.CommitAsync();
            return result;
        });
    }

    public static TReturn WithClearChangeTrackerOnRetry<TDbContext, TResult, TReturn>(
        this IBuilderWithMiddleware<TDbContext, TResult, TReturn> returnTo)
        where TReturn : IBuilderWithMiddleware<TDbContext, TResult, TReturn> where TDbContext : DbContext
    {
        return returnTo.WithMiddleware(async (next, args) =>
        {
            args.Context.ChangeTracker.Clear();
            return await next(args);
        });
    }
}