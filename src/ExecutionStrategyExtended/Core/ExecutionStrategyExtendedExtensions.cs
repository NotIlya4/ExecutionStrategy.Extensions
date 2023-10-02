using System.Data;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public static class ExecutionStrategyExtendedExtensions
{
    public static Task<TResponse> ExecuteAsync<TDbContext, TResponse>(
        this IExecutionStrategyExtended<TDbContext> strategyExtended, Func<TDbContext, Task<TResponse>> action) where TDbContext : DbContext
    {
        return strategyExtended.ExecuteAsync(action, strategyExtended.Options.MainContext);
    }
    
    public static async Task ExecuteAsync<TDbContext>(
        this IExecutionStrategyExtended<TDbContext> strategyExtended, TDbContext mainContext, Func<TDbContext, Task> action) where TDbContext : DbContext
    {
        await strategyExtended.ExecuteAsync(async (context) =>
        {
            await action(context);
            return true;
        }, mainContext);
    }
    
    public static async Task ExecuteAsync<TDbContext>(
        this IExecutionStrategyExtended<TDbContext> strategyExtended, Func<TDbContext, Task> action) where TDbContext : DbContext
    {
        await strategyExtended.ExecuteAsync(async (context) =>
        {
            await action(context);
            return true;
        });
    }

    public static Task<TResponse> ExecuteInTransactionAsync<TDbContext, TResponse>(
        this IExecutionStrategyExtended<TDbContext> strategyExtended, Func<TDbContext, Task<TResponse>> action, IsolationLevel isolationLevel)
        where TDbContext : DbContext
    {
        return strategyExtended.ExecuteInTransactionAsync(action, strategyExtended.Options.MainContext, isolationLevel);
    }

    public static Task ExecuteInTransactionAsync<TDbContext>(
        this IExecutionStrategyExtended<TDbContext> strategyExtended, Func<TDbContext, Task> action, TDbContext mainContext, IsolationLevel isolationLevel)
        where TDbContext : DbContext
    {
        return strategyExtended.ExecuteInTransactionAsync(async (context) =>
        {
            await action(context);
            return true;
        }, mainContext, isolationLevel);
    }

    public static Task ExecuteInTransactionAsync<TDbContext>(
        this IExecutionStrategyExtended<TDbContext> strategyExtended, Func<TDbContext, Task> action, IsolationLevel isolationLevel)
        where TDbContext : DbContext
    {
        return strategyExtended.ExecuteInTransactionAsync(async (context) =>
        {
            await action(context);
            return true;
        }, isolationLevel);
    }
}