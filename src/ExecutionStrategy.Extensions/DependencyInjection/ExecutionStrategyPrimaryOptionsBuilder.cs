using EntityFrameworkCore.ExecutionStrategy.Extensions.Container;
using EntityFrameworkCore.ExecutionStrategy.Extensions.Internal;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;

internal class ExecutionStrategyPrimaryOptionsBuilder<TDbContext> : IExecutionStrategyPrimaryOptionsBuilder<TDbContext>
    where TDbContext : DbContext
{
    private readonly DependenciesContainer _container;
    public IExecutionStrategyData Data => _container.Data;

    public ExecutionStrategyPrimaryOptionsBuilder(DependenciesContainer container)
    {
        _container = container;
    }

    public IExecutionStrategyPrimaryOptionsBuilder<TDbContext> WithMiddleware(
        ExecutionStrategyMiddleware<TDbContext, object> middleware)
    {
        _container.MiddlewaresManager.Middlewares.Add(middleware.ToGeneric());
        return this;
    }
}

public interface IExecutionStrategyPrimaryOptionsBuilder<TDbContext> :
    IBuilderWithMiddleware<TDbContext, object, IExecutionStrategyPrimaryOptionsBuilder<TDbContext>>, IBuilderWithData
    where TDbContext : DbContext
{
}