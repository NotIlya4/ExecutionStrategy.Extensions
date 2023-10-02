using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using EntityFrameworkCore.ExecutionStrategyExtended.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Options;

public class ExecutionStrategyExtendedOptionsBuilder<TDbContext> where TDbContext : DbContext
{
    public ExecutionStrategyExtendedOptions<TDbContext> Options { get; set; }

    public IServiceProvider ServiceProvider
    {
        get => Options.Data.ServiceProvider();
        set => Options.Data.ServiceProvider(value);
    }

    public DbContextRetryBehaviorBuilderPart<TDbContext> DbContextRetryBehavior { get; set; }

    public ExecutionStrategyExtendedOptionsBuilder(IServiceProvider serviceProvider,
        ActualDbContextProvider<TDbContext> actualDbContextProvider,
        ExecutionStrategyExtendedOptions<TDbContext> options)
    {
        options.Data.ActualDbContextProvider(actualDbContextProvider);

        ServiceProvider = serviceProvider;
        Options = options;
        DbContextRetryBehavior = new DbContextRetryBehaviorBuilderPart<TDbContext>(this);

        DbContextRetryBehavior.UseClearChangeTrackerRetryBehavior();
    }
}