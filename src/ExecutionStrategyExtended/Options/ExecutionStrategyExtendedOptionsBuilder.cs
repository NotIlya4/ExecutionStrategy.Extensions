using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using EntityFrameworkCore.ExecutionStrategyExtended.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Options;

public class ExecutionStrategyExtendedOptionsBuilder<TDbContext> where TDbContext : DbContext
{
    public ExecutionStrategyExtendedOptions<TDbContext> Options { get; }
    public IServiceProvider ServiceProvider { get; }
    public DbContextRetryBehaviorBuilderPart<TDbContext> DbContextRetryBehavior { get; }
    
    public ExecutionStrategyExtendedOptionsBuilder(IServiceProvider serviceProvider, ExecutionStrategyExtendedOptions<TDbContext> options, TDbContext mainContext, ActualDbContextProvider<TDbContext> actualDbContextProvider)
    {
        options.MainContext = mainContext;
        options.ActualDbContextProvider = actualDbContextProvider;
        
        ServiceProvider = serviceProvider;
        Options = options;
        DbContextRetryBehavior = new DbContextRetryBehaviorBuilderPart<TDbContext>(this);
    }
}