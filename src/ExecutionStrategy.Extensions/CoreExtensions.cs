using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <summary>
/// Class that contains core extension. Any other extensions are built on top of it.
/// </summary>
public static class CoreExtensions
{
    /// <summary>
    /// Executes your <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Operation"/> from <see cref="IExecutionStrategyOptions{TDbContext,TResult}"/> wrapped with <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Middlewares"/>.
    /// 
    /// This is core extension. Any other extensions are built on top of it.
    /// </summary>
    /// <param name="context">DbContext instance that will be used as a source for <see cref="IExecutionStrategy"/></param>
    /// <param name="options">Options instance</param>
    /// <typeparam name="TDbContext">Your type of DbContext</typeparam>
    /// <typeparam name="TResult">Return type of <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Operation"/>
    /// from <see cref="IExecutionStrategyOptions{TDbContext,TResult}"/></typeparam>
    /// <returns>Result that you return from <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Operation"/>
    /// of <see cref="IExecutionStrategyOptions{TDbContext,TResult}"/></returns>
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