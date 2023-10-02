using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Configuration;

public record ExecutionStrategyExtendedOptions<TDbContext> where TDbContext : DbContext
{
    public Dictionary<object, object> Data { get; } = new();

    public TDbContext MainContext
    {
        get => Data.MainContext<TDbContext>();
        set => Data.MainContext(value);
    }

    public IDbContextRetryBehaviorFactory<TDbContext> DbContextRetryBehaviorFactory
    {
        get => Data.RetryBehaviorFactory<TDbContext>();
        set => Data.RetryBehaviorFactory(value);
    }

    public ActualDbContextProvider<TDbContext> ActualDbContextProvider
    {
        get => Data.DbContextProvider<TDbContext>();
        set => Data.DbContextProvider(value);
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