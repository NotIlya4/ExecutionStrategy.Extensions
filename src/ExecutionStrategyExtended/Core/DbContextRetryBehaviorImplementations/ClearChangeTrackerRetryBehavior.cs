using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core.DbContextRetryBehaviorImplementations;

internal class ClearChangeTrackerRetryBehavior<TDbContext> : IDbContextRetryBehavior<TDbContext>
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public ClearChangeTrackerRetryBehavior(TDbContext context)
    {
        _context = context;
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _context.Database.CreateExecutionStrategy();
    }

    public Task<TDbContext> ProvideDbContextForRetry(int attempt)
    {
        _context.ChangeTracker.Clear();
        return Task.FromResult(_context);
    }
}