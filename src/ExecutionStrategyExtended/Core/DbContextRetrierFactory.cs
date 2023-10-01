using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;
using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier.RetrierTypes;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

internal class DbContextRetrierFactory<TDbContext> where TDbContext : DbContext
{
    private readonly DbContextRetrierConfiguration _retrierConfiguration;
    private readonly IDbContextFactory<TDbContext> _dbContextFactory;

    public DbContextRetrierFactory(DbContextRetrierConfiguration retrierConfiguration, IDbContextFactory<TDbContext> dbContextFactory)
    {
        _retrierConfiguration = retrierConfiguration;
        _dbContextFactory = dbContextFactory;
    }

    public IDbContextRetrier<TDbContext> Create(TDbContext mainContext)
    {
        return _retrierConfiguration.DbContextRetrierType switch
        {
            DbContextRetrierType.CreateNew =>
                new CreateNewDbContextRetrier<TDbContext>(_retrierConfiguration.DisposePreviousContext,
                    _dbContextFactory, mainContext),
            DbContextRetrierType.ClearChangeTracker => new ClearChangeTrackerRetrier<TDbContext>(mainContext),
            DbContextRetrierType.UseSame => new UseSameDbContextRetrier<TDbContext>(mainContext),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}