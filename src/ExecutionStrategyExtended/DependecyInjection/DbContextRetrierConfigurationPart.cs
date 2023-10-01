using EntityFrameworkCore.ExecutionStrategyExtended.Configuration;
using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;

public struct DbContextRetrierConfigurationPart
{
    private readonly IExecutionStrategyExtendedConfiguration _configuration;

    public DbContextRetrierConfigurationPart(IExecutionStrategyExtendedConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public IExecutionStrategyExtendedConfiguration UseNewDbContextRetrier(bool disposePreviousContext = true)
    {
        _configuration.RetrierConfiguration(DbContextRetriers.NewDbContextRetrier(disposePreviousContext));
        return _configuration;
    }
    
    public IExecutionStrategyExtendedConfiguration UseClearChangeTrackerRetrier()
    {
        _configuration.RetrierConfiguration(DbContextRetriers.ClearChangeTrackerRetrier());
        return _configuration;
    }
    
    public IExecutionStrategyExtendedConfiguration UseUseSameDbContextRetrier()
    {
        _configuration.RetrierConfiguration(DbContextRetriers.UseSameDbContextRetrier());
        return _configuration;
    }
}