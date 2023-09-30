using EntityFrameworkCore.ExecutionStrategyExtended.Builders;
using EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

public static class ExecutionStrategyExtendedConfigurationExtensions
{
    public static IExecutionStrategyConfigurationBuilder UseDefaultPostgresDetector(
        this IBuilderPropertySetter<IIdempotenceViolationDetector, IExecutionStrategyConfigurationBuilder> builder,
        IdempotencyTokenTableConfiguration? tableConfiguration = null)
    {
        tableConfiguration ??= new IdempotencyTokenTableConfiguration();

        return builder.Set(new DefaultPostgresDetector(tableConfiguration));
    }

    public static IExecutionStrategyConfigurationBuilder UseNewDbContextRetrier(
        this IBuilderPropertySetter<DbContextRetrierConfiguration, IExecutionStrategyConfigurationBuilder> builder,
        bool disposePreviousContext = true)
    {
        return builder.Set(DbContextRetriers.NewDbContextRetrier(disposePreviousContext));
    }
    
    public static IExecutionStrategyConfigurationBuilder UseClearChangeTrackerRetrier(
        this IBuilderPropertySetter<DbContextRetrierConfiguration, IExecutionStrategyConfigurationBuilder> builder)
    {
        return builder.Set(DbContextRetriers.ClearChangeTrackerRetrier());
    }
    
    public static IExecutionStrategyConfigurationBuilder UseUseSameDbContextRetrier(
        this IBuilderPropertySetter<DbContextRetrierConfiguration, IExecutionStrategyConfigurationBuilder> builder)
    {
        return builder.Set(DbContextRetriers.UseSameDbContextRetrier());
    }
}