using EntityFrameworkCore.ExecutionStrategyExtended.Configuration;
using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;

internal static class InternalOptionsExtensions
{
    public static IDbContextFactory<TDbContext> DbContextFactory<TDbContext>(this ExecutionStrategyExtendedOptions optionsBuilder) where TDbContext : DbContext
    {
        return (IDbContextFactory<TDbContext>)optionsBuilder.Data[nameof(IDbContextFactory<TDbContext>)];
    }
    
    public static void DbContextFactory<TDbContext>(this ExecutionStrategyExtendedOptions optionsBuilder, IDbContextFactory<TDbContext> factory) where TDbContext : DbContext
    {
        optionsBuilder.Data[nameof(IDbContextFactory<TDbContext>)] = factory;
    }
    
    public static TDbContext MainContext<TDbContext>(this ExecutionStrategyExtendedOptions optionsBuilder) where TDbContext : DbContext
    {
        return (TDbContext)optionsBuilder.Data[nameof(TDbContext)];
    }
    
    public static void MainContext<TDbContext>(this ExecutionStrategyExtendedOptions optionsBuilder, TDbContext mainContext) where TDbContext : DbContext
    {
        optionsBuilder.Data[nameof(TDbContext)] = mainContext;
    }
    
    public static DbContextRetryBehaviorOptions RetryBehaviorOptions(this ExecutionStrategyExtendedOptions optionsBuilder)
    {
        return (DbContextRetryBehaviorOptions)optionsBuilder.Data[nameof(DbContextRetryBehaviorOptions)];
    }
    
    public static void RetryBehaviorOptions(this ExecutionStrategyExtendedOptions optionsBuilder, DbContextRetryBehaviorOptions retryBehaviorOptions)
    {
        optionsBuilder.Data[nameof(DbContextRetryBehaviorOptions)] = retryBehaviorOptions;
    }
}