using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <summary>
/// Builder with verify succeeded.
/// </summary>
/// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
/// <typeparam name="TResult">Return type of <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Operation"/>.</typeparam>
/// <typeparam name="TReturn">Builder type.</typeparam>
public interface IBuilderWithVerifySucceeded<out TDbContext, TResult, out TReturn> where TDbContext : DbContext
{
    /// <summary>
    /// Provides verify succeeded that will be passed to <see cref="IExecutionStrategy"/>.
    /// </summary>
    /// <param name="verifySucceeded">Verify succeeded.</param>
    /// <returns>Builder.</returns>
    TReturn WithVerifySucceeded(ExecutionStrategyNext<TDbContext, ExecutionResult<TResult>> verifySucceeded);
}

/// <summary>
/// <see cref="IBuilderWithVerifySucceeded{TDbContext,TResult,TReturn}"/> extensions.
/// </summary>
public static class BuilderWithVerifySucceededExtensions
{
    /// <summary>
    /// Provides verify succeeded that will be passed to <see cref="IExecutionStrategy"/>.
    /// </summary>
    /// <param name="builder">Builder.</param>
    /// <param name="verifySucceeded">Verify succeeded.</param>
    /// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
    /// <typeparam name="TResult">Return type of <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Operation"/>.</typeparam>
    /// <typeparam name="TReturn">Builder type.</typeparam>
    /// <returns>Builder.</returns>
    public static TReturn WithVerifySucceeded<TDbContext, TResult, TReturn>(
        this IBuilderWithVerifySucceeded<TDbContext, TResult, TReturn> builder,
        ExecutionStrategyNext<TDbContext, TResult?> verifySucceeded)
        where TReturn : IBuilderWithVerifySucceeded<TDbContext, TResult, TReturn> where TDbContext : DbContext
    {
        return builder.WithVerifySucceeded(async args =>
        {
            var result = await verifySucceeded(args);
            return new ExecutionResult<TResult>(result is not null, result!);
        });
    }
    
    /// <summary>
    /// Provides verify succeeded that will be passed to <see cref="IExecutionStrategy"/>.
    /// </summary>
    /// <param name="builder">Builder.</param>
    /// <param name="verifySucceeded">Verify succeeded.</param>
    /// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
    /// <typeparam name="TReturn">Builder type.</typeparam>
    /// <returns>Builder.</returns>
    public static TReturn WithVerifySucceeded<TDbContext, TReturn>(
        this IBuilderWithVerifySucceeded<TDbContext, Void, TReturn> builder,
        ExecutionStrategyNext<TDbContext, bool> verifySucceeded)
        where TReturn : IBuilderWithVerifySucceeded<TDbContext, Void, TReturn> where TDbContext : DbContext
    {
        return builder.WithVerifySucceeded(async args =>
        {
            var result = await verifySucceeded(args);
            return new ExecutionResult<Void>(result, Void.Instance);
        });
    }
}