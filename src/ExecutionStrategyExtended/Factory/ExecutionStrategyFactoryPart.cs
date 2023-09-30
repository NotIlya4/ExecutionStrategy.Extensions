using EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;
using EntityFrameworkCore.ExecutionStrategyExtended.RetrierTypes;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

internal class ExecutionStrategyFactoryPart<TDbContext> where TDbContext : DbContext
{
    private readonly IExecutionStrategyExtendedConfiguration _extendedConfiguration;
    private readonly IDbContextFactory<TDbContext> _dbContextFactory;

    public ExecutionStrategyFactoryPart(IExecutionStrategyExtendedConfiguration extendedConfiguration, IDbContextFactory<TDbContext> dbContextFactory)
    {
        _extendedConfiguration = extendedConfiguration;
        _dbContextFactory = dbContextFactory;
    }
    
    public IDbContextRetrier<TDbContext> CreateDbContextRetrier(TDbContext mainContext)
    {
        return _extendedConfiguration.DbContextRetrierConfiguration.DbContextRetrierType switch
        {
            DbContextRetrierType.CreateNew => new CreateNewDbContextRetrier<TDbContext>(
                _extendedConfiguration.DbContextRetrierConfiguration.DisposePreviousContext, _dbContextFactory, mainContext),
            DbContextRetrierType.ClearChangeTracker => new ClearChangeTrackerRetrier<TDbContext>(mainContext),
            DbContextRetrierType.UseSame => new UseSameDbContextRetrier<TDbContext>(mainContext),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}