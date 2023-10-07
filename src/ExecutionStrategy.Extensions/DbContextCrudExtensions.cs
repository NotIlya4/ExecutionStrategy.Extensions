using EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

internal static class DbContextCrudExtensions
{
    public static List<ExecutionStrategyMiddleware<TDbContext, TResult>>
        GetMiddlewares<TDbContext, TResult>(this DbContext context) where TDbContext : DbContext
    {
        return context.GetService<ExecutionStrategyContainer>().MiddlewaresManager
            .CastMiddlewares<TDbContext, TResult>().ToList();
    }

    public static IExecutionStrategyData GetData(this DbContext context)
    {
        return context.GetService<ExecutionStrategyContainer>().Data;
    }

    public static IExecutionStrategyOptions<TDbContext, TResult> CreateOptionsFromPrimary<TDbContext, TResult>(
        this DbContext context, ExecutionStrategyOperation<TDbContext, TResult> operation) where TDbContext : DbContext
    {
        return new ExecutionStrategyOptions<TDbContext, TResult>(
            context.GetData(),
            context.GetMiddlewares<TDbContext, TResult>(),
            operation,
            default,
            null);
    }
}