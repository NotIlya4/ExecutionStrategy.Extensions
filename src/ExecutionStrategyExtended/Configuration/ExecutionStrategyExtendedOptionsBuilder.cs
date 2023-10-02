using EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Configuration;

public class ExecutionStrategyExtendedOptionsBuilder<TDbContext> where TDbContext : DbContext
{
    public ExecutionStrategyExtendedOptions<TDbContext> Options { get; }
    public IServiceProvider ServiceProvider { get; }

    public ExecutionStrategyExtendedOptionsBuilder(IServiceProvider serviceProvider) : this(serviceProvider, new ExecutionStrategyExtendedOptions<TDbContext>())
    {
        
    }
    
    public ExecutionStrategyExtendedOptionsBuilder(IServiceProvider serviceProvider, ExecutionStrategyExtendedOptions<TDbContext> options)
    {
        ServiceProvider = serviceProvider;
        Options = options;
        DbContextRetryBehavior = new DbContextRetryBehaviorBuilderPart<TDbContext>(this);
    }
    
    public DbContextRetryBehaviorBuilderPart<TDbContext> DbContextRetryBehavior { get; }
}