using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Options;

public record ExecutionStrategyExtendedOptions<TDbContext> where TDbContext : DbContext
{
    public ExecutionStrategyExtendedData Data { get; }

    public ActualDbContextProvider<TDbContext> ActualDbContextProvider
    {
        get => Data.ActualDbContextProvider<TDbContext>();
        set => Data.ActualDbContextProvider(value);
    }

    public IDbContextRetryBehaviorFactory<TDbContext> DbContextRetryBehaviorFactory
    {
        get => Data.DbContextRetryBehaviorFactory<TDbContext>();
        set => Data.DbContextRetryBehaviorFactory(value);
    }

    internal ExecutionStrategyExtendedOptions()
    {
        Data = new ExecutionStrategyExtendedData();
    }

    public ExecutionStrategyExtendedOptions(ActualDbContextProvider<TDbContext> actualDbContextProvider,
        IDbContextRetryBehaviorFactory<TDbContext> retryBehaviorFactory) : this(new ExecutionStrategyExtendedData(),
        actualDbContextProvider, retryBehaviorFactory)
    {
    }

    public ExecutionStrategyExtendedOptions(ExecutionStrategyExtendedData data,
        ActualDbContextProvider<TDbContext> actualDbContextProvider,
        IDbContextRetryBehaviorFactory<TDbContext> retryBehaviorFactory)
    {
        Data = data;
        ActualDbContextProvider = actualDbContextProvider;
        DbContextRetryBehaviorFactory = retryBehaviorFactory;
    }
}