using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

internal class ExecutionStrategyMiddlewaresManager
{
    public List<ExecutionStrategyMiddleware<DbContext, object>> Middlewares { get; } = new();

    public IEnumerable<ExecutionStrategyMiddleware<TDbContext, TResult>> CastMiddlewares<TDbContext, TResult>()
        where TDbContext : DbContext
    {
        foreach (var middleware in Middlewares)
        {
            yield return middleware.FromGeneric<TDbContext, TResult>();
        }
    }
}