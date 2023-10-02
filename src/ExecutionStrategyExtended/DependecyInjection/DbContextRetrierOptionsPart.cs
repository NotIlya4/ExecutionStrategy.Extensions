using EntityFrameworkCore.ExecutionStrategyExtended.Configuration;
using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;

public struct DbContextRetrierOptionsPart
{
    private readonly ExecutionStrategyExtendedOptions _options;

    internal DbContextRetrierOptionsPart(ExecutionStrategyExtendedOptions options)
    {
        _options = options;
    }
    
    public IExecutionStrategyExtendedOptionsBuilder UseCreateNewDbContextRetrier(bool disposePreviousContext = true)
    {
        _options.RetryBehaviorOptions(DbContextRetryBehaviors.CreateNewDbContextRetrier(disposePreviousContext));
        return _options;
    }
    
    public IExecutionStrategyExtendedOptionsBuilder UseClearChangeTrackerRetrier()
    {
        _options.RetryBehaviorOptions(DbContextRetryBehaviors.ClearChangeTrackerRetrier());
        return _options;
    }
    
    public IExecutionStrategyExtendedOptionsBuilder UseUseSameDbContextRetrier()
    {
        _options.RetryBehaviorOptions(DbContextRetryBehaviors.UseSameDbContextRetrier());
        return _options;
    }
}