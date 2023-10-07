using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

internal class ExecutionStrategyMiddlewaresManager
{
    public List<ExecutionStrategyMiddleware<DbContext, object>> LowLevelMiddlewares { get; } = new();

    public IEnumerable<ExecutionStrategyMiddleware<TDbContext, TResult>> CastMiddlewares<TDbContext, TResult>()
        where TDbContext : DbContext
    {
        foreach (var lowLevelMiddleware in LowLevelMiddlewares)
        {
            yield return lowLevelMiddleware.CastMiddleware<TDbContext, TResult>();
        }
    }

    public void AddLowLevel(ExecutionStrategyMiddleware<DbContext, object> lowLevelMiddleware)
    {
        LowLevelMiddlewares.Add(lowLevelMiddleware);
    }
}