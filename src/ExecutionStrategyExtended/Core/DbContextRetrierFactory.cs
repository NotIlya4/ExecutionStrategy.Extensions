using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;
using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier.RetrierTypes;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

internal class DbContextRetrierFactory<TDbContext> where TDbContext : DbContext
{
    private readonly DbContextRetryBehaviorOptions _retryBehaviorOptions;
    private readonly IDbContextFactory<TDbContext> _dbContextFactory;

    public DbContextRetrierFactory(DbContextRetryBehaviorOptions retryBehaviorOptions, IDbContextFactory<TDbContext> dbContextFactory)
    {
        _retryBehaviorOptions = retryBehaviorOptions;
        _dbContextFactory = dbContextFactory;
    }

    public IDbContextRetryBehavior<TDbContext> Create(TDbContext mainContext)
    {
        return _retryBehaviorOptions.DbContextRetryBehaviorType switch
        {
            DbContextRetryBehaviorType.CreateNew =>
                new CreateNewDbContextRetryBehavior<TDbContext>(_retryBehaviorOptions.DisposePreviousContext,
                    _dbContextFactory, mainContext),
            DbContextRetryBehaviorType.ClearChangeTracker => new ClearChangeTrackerRetryBehavior<TDbContext>(mainContext),
            DbContextRetryBehaviorType.UseSame => new UseSameDbContextRetryBehavior<TDbContext>(mainContext),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}