using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;

internal class ExecutionStrategyPrimaryOptionsBuilder<TDbContext> : IExecutionStrategyPrimaryOptionsBuilder<TDbContext>
    where TDbContext : DbContext
{
    private readonly ExecutionStrategyContainer _container;
    public IExecutionStrategyData Data => _container.Data;

    public ExecutionStrategyPrimaryOptionsBuilder(ExecutionStrategyContainer container)
    {
        _container = container;
    }

    public IExecutionStrategyPrimaryOptionsBuilder<TDbContext> WithMiddleware(
        ExecutionStrategyMiddleware<TDbContext, Task<object>> middleware)
    {
        _container.MiddlewaresManager.LowLevelMiddlewares.Add(middleware.CastMiddleware());
        return this;
    }
}

public interface IExecutionStrategyPrimaryOptionsBuilder<TDbContext>
    : IBuilderWithMiddleware<TDbContext, object, IExecutionStrategyPrimaryOptionsBuilder<TDbContext>>, 
        IBuilderWithData
    where TDbContext : DbContext
{
}