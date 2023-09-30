using EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

internal class MainFactory<TDbContext> where TDbContext : DbContext
{
    public MainFactory(IExecutionStrategyExtendedConfiguration extendedConfiguration, IDbContextFactory<TDbContext> factory)
    {
        IdempotencyToken = new IdempotencyTokenFactoryPart(extendedConfiguration);
        ExecutionStrategy = new ExecutionStrategyFactoryPart<TDbContext>(extendedConfiguration, factory);
    }
    
    public IdempotencyTokenFactoryPart IdempotencyToken { get; }
    public ExecutionStrategyFactoryPart<TDbContext> ExecutionStrategy { get; }
}