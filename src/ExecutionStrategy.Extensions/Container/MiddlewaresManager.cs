using EntityFrameworkCore.ExecutionStrategy.Extensions.Internal;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.Container;

internal class MiddlewaresManager
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