using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public class DbContextRetryBehaviorDelegateFactory<TDbContext> : IDbContextRetryBehaviorFactory<TDbContext> where TDbContext : DbContext
{
    private readonly Func<TDbContext, IDbContextRetryBehavior<TDbContext>> _factory;

    public DbContextRetryBehaviorDelegateFactory(Func<TDbContext, IDbContextRetryBehavior<TDbContext>> factory)
    {
        _factory = factory;
    }
    
    public IDbContextRetryBehavior<TDbContext> Create(TDbContext mainContext)
    {
        return _factory(mainContext);
    }
}