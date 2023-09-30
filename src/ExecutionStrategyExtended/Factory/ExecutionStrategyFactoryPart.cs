using EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;
using EntityFrameworkCore.ExecutionStrategyExtended.RetrierTypes;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

internal class ExecutionStrategyFactoryPart<TDbContext> where TDbContext : DbContext
{
    private readonly IExecutionStrategyInternalConfiguration _configuration;
    private readonly IDbContextFactory<TDbContext> _dbContextFactory;

    public ExecutionStrategyFactoryPart(IExecutionStrategyInternalConfiguration configuration, IDbContextFactory<TDbContext> dbContextFactory)
    {
        _configuration = configuration;
        _dbContextFactory = dbContextFactory;
    }
    
    public IDbContextRetrier<TDbContext> CreateDbContextRetrier(TDbContext mainContext)
    {
        return _configuration.DbContextRetrierConfiguration.DbContextRetrierType switch
        {
            DbContextRetrierType.CreateNew => new CreateNewDbContextRetrier<TDbContext>(
                _configuration.DbContextRetrierConfiguration.DisposePreviousContext, _dbContextFactory, mainContext),
            DbContextRetrierType.ClearChangeTracker => new ClearChangeTrackerRetrier<TDbContext>(mainContext),
            DbContextRetrierType.UseSame => new UseSameDbContextRetrier<TDbContext>(mainContext),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}