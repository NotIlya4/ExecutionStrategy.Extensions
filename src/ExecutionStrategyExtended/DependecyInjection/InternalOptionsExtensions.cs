using EntityFrameworkCore.ExecutionStrategyExtended.Configuration;
using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;

internal static class InternalOptionsExtensions
{
    public static IDbContextFactory<TDbContext> DbContextFactory<TDbContext>(this Dictionary<object, object> data) where TDbContext : DbContext
    {
        return (IDbContextFactory<TDbContext>)data[nameof(IDbContextFactory<TDbContext>)];
    }
    
    public static void DbContextFactory<TDbContext>(this Dictionary<object, object> data, IDbContextFactory<TDbContext> factory) where TDbContext : DbContext
    {
        data[nameof(IDbContextFactory<TDbContext>)] = factory;
    }
    
    public static TDbContext MainContext<TDbContext>(this Dictionary<object, object> data) where TDbContext : DbContext
    {
        return (TDbContext)data[nameof(TDbContext)];
    }
    
    public static void MainContext<TDbContext>(this Dictionary<object, object> data, TDbContext mainContext) where TDbContext : DbContext
    {
        data[nameof(TDbContext)] = mainContext;
    }
    
    public static IDbContextRetryBehaviorFactory<TDbContext> RetryBehaviorFactory<TDbContext>(this Dictionary<object, object> data) where TDbContext : DbContext
    {
        return (IDbContextRetryBehaviorFactory<TDbContext>)data[nameof(IDbContextRetryBehaviorFactory<TDbContext>)];
    }
    
    public static void RetryBehaviorFactory<TDbContext>(this Dictionary<object, object> data, IDbContextRetryBehaviorFactory<TDbContext> factory) where TDbContext : DbContext
    {
        data[nameof(IDbContextRetryBehaviorFactory<TDbContext>)] = factory;
    }
    
    public static ActualDbContextProvider<TDbContext> DbContextProvider<TDbContext>(this Dictionary<object, object> data) where TDbContext : DbContext
    {
        return (ActualDbContextProvider<TDbContext>)data[nameof(ActualDbContextProvider<TDbContext>)];
    }
    
    public static void DbContextProvider<TDbContext>(this Dictionary<object, object> data, ActualDbContextProvider<TDbContext> factory) where TDbContext : DbContext
    {
        data[nameof(ActualDbContextProvider<TDbContext>)] = factory;
    }
}