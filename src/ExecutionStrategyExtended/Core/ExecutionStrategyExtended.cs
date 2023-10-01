using System.Data;
using EntityFrameworkCore.ExecutionStrategyExtended.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

internal class ExecutionStrategyExtended<TDbContext> : IExecutionStrategyExtended<TDbContext>
    where TDbContext : DbContext
{
    private readonly DbContextRetrierFactory<TDbContext> _retrierFactory;
    private readonly ActualDbContextProvider<TDbContext> _actualDbContextProvider;
    public TDbContext MainContext { get; }
    public IExecutionStrategyExtendedConfiguration Configuration { get; }

    public ExecutionStrategyExtended(DbContextRetrierFactory<TDbContext> retrierFactory, TDbContext mainContext,
        IExecutionStrategyExtendedConfiguration configuration,
        ActualDbContextProvider<TDbContext> actualDbContextProvider)
    {
        _retrierFactory = retrierFactory;
        MainContext = mainContext;
        Configuration = configuration;
        _actualDbContextProvider = actualDbContextProvider;
    }

    public async Task<TResponse> ExecuteAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext)
    {
        _actualDbContextProvider.DbContext = mainContext;

        var retrier = _retrierFactory.Create(mainContext);
        var strategy = retrier.CreateExecutionStrategy();
        var retryNumber = 1;

        return await strategy.ExecuteAsync(
            async () =>
            {
                var context = await retrier.ProvideDbContextForRetry(retryNumber);
                _actualDbContextProvider.DbContext = context;

                retryNumber += 1;
                return await action(context);
            });
    }

    public async Task<TResponse> ExecuteInTransactionAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext, IsolationLevel isolationLevel)
    {
        return await ExecuteAsync(async context =>
        {
            await using var transaction = await context.Database.BeginTransactionAsync(isolationLevel);

            var response = await action(context);

            await transaction.CommitAsync();

            return response;
        }, mainContext);
    }
}