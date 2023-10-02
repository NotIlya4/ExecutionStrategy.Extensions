using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier.RetrierTypes;

internal class UseSameDbContextRetryBehavior<TDbContext> : IDbContextRetryBehavior<TDbContext>
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public UseSameDbContextRetryBehavior(TDbContext context)
    {
        _context = context;
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _context.Database.CreateExecutionStrategy();
    }

    public Task<TDbContext> ProvideDbContextForRetry(int attempt)
    {
        return Task.FromResult(_context);
    }
}