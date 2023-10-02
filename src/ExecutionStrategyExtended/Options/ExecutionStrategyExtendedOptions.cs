using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Options;

public record ExecutionStrategyExtendedOptions<TDbContext> where TDbContext : DbContext
{
    public ExecutionStrategyExtendedData Data { get; } = new();

    public TDbContext MainContext
    {
        get => Data.MainContext<TDbContext>();
        set => Data.MainContext(value);
    }

    public IDbContextRetryBehaviorFactory<TDbContext> DbContextRetryBehaviorFactory
    {
        get => Data.DbContextRetryBehaviorFactory<TDbContext>();
        set => Data.DbContextRetryBehaviorFactory(value);
    }

    public ActualDbContextProvider<TDbContext> ActualDbContextProvider
    {
        get => Data.ActualDbContextProvider<TDbContext>();
        set => Data.ActualDbContextProvider(value);
    }

    internal ExecutionStrategyExtendedOptions()
    {
        
    }

    public ExecutionStrategyExtendedOptions(TDbContext mainContext, IDbContextRetryBehaviorFactory<TDbContext> retryBehaviorFactory, ActualDbContextProvider<TDbContext> actualDbContextProvider)
    {
        MainContext = mainContext;
        DbContextRetryBehaviorFactory = retryBehaviorFactory;
        ActualDbContextProvider = actualDbContextProvider;
    }
}