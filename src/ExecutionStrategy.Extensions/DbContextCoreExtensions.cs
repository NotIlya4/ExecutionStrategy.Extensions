using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public static class DbContextCoreExtensions
{
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

    private static ExecutionStrategyOperation<TDbContext, TResult> WrapMiddlewares<TDbContext, TResult>(
        IEnumerable<ExecutionStrategyMiddleware<TDbContext, TResult>> middlewares,
        ExecutionStrategyOperation<TDbContext, TResult> operation) where TDbContext : DbContext
    {
        var resultOperation = operation;

        foreach (var middleware in middlewares)
        {
            resultOperation = MiddlewareToOperation(middleware, resultOperation);
        }

        return resultOperation;
    }

    private static ExecutionStrategyOperation<TDbContext, TResult> MiddlewareToOperation<TDbContext, TResult>(
        ExecutionStrategyMiddleware<TDbContext, TResult> middleware,
        ExecutionStrategyOperation<TDbContext, TResult> operation) where TDbContext : DbContext
    {
        return (args) => middleware(operation, args);
    }
}