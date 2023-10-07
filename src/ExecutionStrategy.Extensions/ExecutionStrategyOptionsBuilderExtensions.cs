using System.Data;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public static class ExecutionStrategyOptionsBuilderExtensions
{
    public static TReturn WithTransaction<TDbContext, TResult, TReturn>(this IBuilderWithMiddleware<TDbContext, TResult, TReturn> returnTo, IsolationLevel isolationLevel)
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

    public static TReturn WithClearChangeTrackerOnRetry<TDbContext, TResult, TReturn>(this IBuilderWithMiddleware<TDbContext, TResult, TReturn> returnTo)
        where TReturn : IBuilderWithMiddleware<TDbContext, TResult, TReturn> where TDbContext : DbContext
    {
        return returnTo.WithMiddleware(async (next, args) =>
        {
            args.Context.ChangeTracker.Clear();
            return await next(args);
        });
    }

    public static TReturn WithData<TReturn>(this TReturn returnTo, Action<IExecutionStrategyData> action) where TReturn : IBuilderWithData
    {
        action(returnTo.Data);
        return returnTo;
    }
}