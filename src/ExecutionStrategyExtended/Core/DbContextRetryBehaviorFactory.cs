using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;
using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier.RetrierTypes;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

internal class DbContextRetryBehaviorFactory<TDbContext> : IDbContextRetryBehaviorFactory<TDbContext> where TDbContext : DbContext
{
    private readonly DbContextRetryBehaviorType _dbContextRetryBehaviorType;
    private readonly bool _disposePreviousContext;
    private readonly IDbContextFactory<TDbContext> _dbContextFactory;

    public DbContextRetryBehaviorFactory(DbContextRetryBehaviorType retryBehaviorType, bool disposePreviousContext, IDbContextFactory<TDbContext> dbContextFactory)
    {
        _dbContextRetryBehaviorType = retryBehaviorType;
        _disposePreviousContext = disposePreviousContext;
        _dbContextFactory = dbContextFactory;
    }

    public IDbContextRetryBehavior<TDbContext> Create(TDbContext mainContext)
    {
        return _dbContextRetryBehaviorType switch
        {
            DbContextRetryBehaviorType.CreateNew =>
                new CreateNewDbContextRetryBehavior<TDbContext>(_disposePreviousContext,
                    _dbContextFactory, mainContext),
            DbContextRetryBehaviorType.ClearChangeTracker => new ClearChangeTrackerRetryBehavior<TDbContext>(mainContext),
            DbContextRetryBehaviorType.UseSame => new UseSameDbContextRetryBehavior<TDbContext>(mainContext),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}