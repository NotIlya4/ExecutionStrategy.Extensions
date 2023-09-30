using EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

internal class MainFactory<TDbContext> where TDbContext : DbContext
{
    public MainFactory(IExecutionStrategyInternalConfiguration configuration, IDbContextFactory<TDbContext> factory)
    {
        IdempotencyToken = new IdempotencyTokenFactoryPart(configuration);
        ExecutionStrategy = new ExecutionStrategyFactoryPart<TDbContext>(configuration, factory);
    }
    
    public IdempotencyTokenFactoryPart IdempotencyToken { get; }
    public ExecutionStrategyFactoryPart<TDbContext> ExecutionStrategy { get; }
}