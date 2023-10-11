using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <summary>
/// Class that contains core extension. Any other extensions are built on top of it.
/// </summary>
public static class CoreExtensions
{
    /// <summary>
    /// Executes provided <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Operation"/> wrapped with <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Middlewares"/> inside <see cref="IExecutionStrategy"/> and if any transient exception occured retries operation.
    /// </summary>
    /// <remarks>This is core extension. Any other extensions are built on top of it.</remarks>
    /// <param name="context">Your context instance.</param>
    /// <param name="options">Options instance</param>
    /// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
    /// <typeparam name="TResult">Return type of <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Operation"/>.</typeparam>
    /// <returns>Result from <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Operation"/>.</returns>
    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(this TDbContext context,
        IExecutionStrategyOptions<TDbContext, TResult> options) where TDbContext : DbContext
    {
        var strategy = context.Database.CreateExecutionStrategy();

        int attempt = 0;

        return strategy.ExecuteAsync(
            true,
            async (_, _, _) =>
            {
                attempt += 1;

                var middlewareArgs = new ExecutionStrategyOperationArgs<TDbContext>(
                    options.Data, context, attempt, options.CancellationToken);

                var result = await WrapMiddlewares(options.Middlewares, options.Operation)(middlewareArgs);

                return result;
            },
            options.VerifySucceeded is not null
                ? async (_, _, _) =>
                {
                    var middlewareArgs = new ExecutionStrategyOperationArgs<TDbContext>(
                        options.Data, context, attempt, options.CancellationToken);
                    return await options.VerifySucceeded(middlewareArgs);
                }
                : null,
            options.CancellationToken);
    }

    internal static ExecutionStrategyNext<TDbContext, TResult> WrapMiddlewares<TDbContext, TResult>(
        IEnumerable<ExecutionStrategyMiddleware<TDbContext, TResult>> middlewares,
        ExecutionStrategyNext<TDbContext, TResult> operation) where TDbContext : DbContext
    {
        var resultOperation = operation;

        foreach (var middleware in middlewares)
        {
            resultOperation = MiddlewareToOperation(middleware, resultOperation);
        }

        return resultOperation;
    }

    internal static ExecutionStrategyNext<TDbContext, TResult> MiddlewareToOperation<TDbContext, TResult>(
        ExecutionStrategyMiddleware<TDbContext, TResult> middleware,
        ExecutionStrategyNext<TDbContext, TResult> operation) where TDbContext : DbContext
    {
        return (args) => middleware(operation, args);
    }
}