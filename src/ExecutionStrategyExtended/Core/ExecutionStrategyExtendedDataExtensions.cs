using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public static class ExecutionStrategyExtendedDataExtensions
{
    public static TDbContext MainContext<TDbContext>(this ExecutionStrategyExtendedData data) where TDbContext : DbContext
    {
        return (TDbContext)data[nameof(TDbContext)];
    }
    
    public static void MainContext<TDbContext>(this ExecutionStrategyExtendedData data, TDbContext mainContext) where TDbContext : DbContext
    {
        data[nameof(TDbContext)] = mainContext;
    }
    
    public static IDbContextRetryBehaviorFactory<TDbContext> DbContextRetryBehaviorFactory<TDbContext>(this ExecutionStrategyExtendedData data) where TDbContext : DbContext
    {
        return (IDbContextRetryBehaviorFactory<TDbContext>)data[nameof(IDbContextRetryBehaviorFactory<TDbContext>)];
    }
    
    public static void DbContextRetryBehaviorFactory<TDbContext>(this ExecutionStrategyExtendedData data, IDbContextRetryBehaviorFactory<TDbContext> factory) where TDbContext : DbContext
    {
        data[nameof(IDbContextRetryBehaviorFactory<TDbContext>)] = factory;
    }
    
    public static ActualDbContextProvider<TDbContext> ActualDbContextProvider<TDbContext>(this ExecutionStrategyExtendedData data) where TDbContext : DbContext
    {
        return (ActualDbContextProvider<TDbContext>)data[nameof(Core.ActualDbContextProvider<TDbContext>)];
    }
    
    public static void ActualDbContextProvider<TDbContext>(this ExecutionStrategyExtendedData data, ActualDbContextProvider<TDbContext> factory) where TDbContext : DbContext
    {
        data[nameof(Core.ActualDbContextProvider<TDbContext>)] = factory;
    }
    
    public static IServiceProvider ServiceProvider(this ExecutionStrategyExtendedData data)
    {
        return (IServiceProvider)data[nameof(IServiceProvider)];
    }
    
    public static void ServiceProvider(this ExecutionStrategyExtendedData data, IServiceProvider serviceProvider)
    {
        data[nameof(IServiceProvider)] = serviceProvider;
    }
}