using System.Data;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <summary>
/// Builder with middlewares.
/// </summary>
/// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
/// <typeparam name="TResult">Return type of <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Operation"/>.</typeparam>
/// <typeparam name="TReturn">Builder type.</typeparam>
public interface IBuilderWithMiddlewares<TDbContext, TResult, out TReturn> 
    where TDbContext : DbContext where TReturn : IBuilderWithMiddlewares<TDbContext, TResult, TReturn>
{
    /// <summary>
    /// Registers middleware that will wrap operation.
    /// </summary>
    /// <param name="middleware">Middleware.</param>
    /// <returns>Builder.</returns>
    TReturn WithMiddleware(ExecutionStrategyMiddleware<TDbContext, TResult> middleware);
    
}

/// <summary>
/// <see cref="IBuilderWithMiddlewares{TDbContext,TResult,TReturn}"/> extensions.
/// </summary>
public static class BuilderWithMiddlewareExtensions
{
    /// <summary>
    /// Registers middleware that will wrap operation.
    /// </summary>
    /// <param name="builder">Builder.</param>
    /// <param name="middleware">Middleware.</param>
    /// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
    /// <typeparam name="TReturn">Builder type.</typeparam>
    /// <returns>Builder.</returns>
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
    
    /// <summary>
    /// Runs operation with specified transaction isolation level.
    /// </summary>
    /// <param name="builder">Builder.</param>
    /// <param name="isolationLevel">Isolation level.</param>
    /// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
    /// <typeparam name="TResult">Return type of <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Operation"/>.</typeparam>
    /// <typeparam name="TReturn">Builder type.</typeparam>
    /// <returns>Builder.</returns>
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

    /// <summary>
    /// Clears change tracker of <typeparamref name="TDbContext"/> before each retry.
    /// </summary>
    /// <param name="builder">Builder.</param>
    /// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
    /// <typeparam name="TResult">Return type of <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Operation"/>.</typeparam>
    /// <typeparam name="TReturn">Builder type.</typeparam>
    /// <returns>Builder.</returns>
    public static TReturn WithClearChangeTrackerOnRetry<TDbContext, TResult, TReturn>(this IBuilderWithMiddlewares<TDbContext, TResult, TReturn> builder)
        where TReturn : IBuilderWithMiddlewares<TDbContext, TResult, TReturn> where TDbContext : DbContext
    {
        return builder.WithMiddleware(async (next, args) =>
        {
            args.Context.ChangeTracker.Clear();
            return await next(args);
        });
    }
}