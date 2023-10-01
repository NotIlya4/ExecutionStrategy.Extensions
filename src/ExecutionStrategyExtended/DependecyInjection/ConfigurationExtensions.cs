using EntityFrameworkCore.ExecutionStrategyExtended.Configuration;
using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;

internal static class ConfigurationExtensions
{
    public static IDbContextFactory<TDbContext> DbContextFactory<TDbContext>(this IExecutionStrategyExtendedConfiguration configuration) where TDbContext : DbContext
    {
        return (IDbContextFactory<TDbContext>)configuration.Data[nameof(IDbContextFactory<TDbContext>)];
    }
    
    public static void DbContextFactory<TDbContext>(this IExecutionStrategyExtendedConfiguration configuration, IDbContextFactory<TDbContext> factory) where TDbContext : DbContext
    {
        configuration.Data[nameof(IDbContextFactory<TDbContext>)] = factory;
    }
    
    public static TDbContext MainContext<TDbContext>(this IExecutionStrategyExtendedConfiguration configuration) where TDbContext : DbContext
    {
        return (TDbContext)configuration.Data[nameof(TDbContext)];
    }
    
    public static void MainContext<TDbContext>(this IExecutionStrategyExtendedConfiguration configuration, TDbContext mainContext) where TDbContext : DbContext
    {
        configuration.Data[nameof(TDbContext)] = mainContext;
    }
    
    public static DbContextRetrierConfiguration RetrierConfiguration(this IExecutionStrategyExtendedConfiguration configuration)
    {
        return (DbContextRetrierConfiguration)configuration.Data[nameof(DbContextRetrierConfiguration)];
    }
    
    public static void RetrierConfiguration(this IExecutionStrategyExtendedConfiguration configuration, DbContextRetrierConfiguration retrierConfiguration)
    {
        configuration.Data[nameof(DbContextRetrierConfiguration)] = retrierConfiguration;
    }
}