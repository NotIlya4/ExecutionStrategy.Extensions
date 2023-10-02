using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;
using EntityFrameworkCore.ExecutionStrategyExtended.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;

public struct DbContextRetryBehaviorBuilderPart<TDbContext> where TDbContext : DbContext
{
    public ExecutionStrategyExtendedOptionsBuilder<TDbContext> Builder { get; }

    internal DbContextRetryBehaviorBuilderPart(ExecutionStrategyExtendedOptionsBuilder<TDbContext> builder)
    {
        Builder = builder;
    }

    public ExecutionStrategyExtendedOptionsBuilder<TDbContext> Use(IDbContextRetryBehaviorFactory<TDbContext> factory)
    {
        Builder.Options.Data.RetryBehaviorFactory(factory);
        return Builder;
    }

    public ExecutionStrategyExtendedOptionsBuilder<TDbContext> UseCreateNewDbContextRetryBehavior(
        bool disposePreviousContext = true)
    {
        return Use(
            new DbContextRetryBehaviorFactory<TDbContext>(DbContextRetryBehaviorType.CreateNew,
                disposePreviousContext, Builder.ServiceProvider.GetRequiredService<IDbContextFactory<TDbContext>>()));
    }

    public ExecutionStrategyExtendedOptionsBuilder<TDbContext> UseClearChangeTrackerRetryBehavior()
    {
        return Use(
            new DbContextRetryBehaviorFactory<TDbContext>(DbContextRetryBehaviorType.ClearChangeTracker, false,
                Builder.ServiceProvider.GetRequiredService<IDbContextFactory<TDbContext>>()));
    }

    public ExecutionStrategyExtendedOptionsBuilder<TDbContext> UseUseSameDbContextRetryBehavior()
    {
        return Use(
            new DbContextRetryBehaviorFactory<TDbContext>(DbContextRetryBehaviorType.UseSame, false,
                Builder.ServiceProvider.GetRequiredService<IDbContextFactory<TDbContext>>()));
    }
}