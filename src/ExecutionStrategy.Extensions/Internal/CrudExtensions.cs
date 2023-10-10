using EntityFrameworkCore.ExecutionStrategy.Extensions.Container;
using EntityFrameworkCore.ExecutionStrategy.Extensions.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.Internal;

internal static class CrudExtensions
{
    public static List<ExecutionStrategyMiddleware<TDbContext, TResult>>
        GetMiddlewares<TDbContext, TResult>(this DbContext context) where TDbContext : DbContext
    {
        return context.GetContainer().MiddlewaresManager.CastMiddlewares<TDbContext, TResult>().ToList();
    }

    public static IExecutionStrategyData GetData(this DbContext context)
    {
        return context.GetContainer().Data;
    }

    public static IExecutionStrategyOptions<TDbContext, TResult> CreateOptionsFromPrimary<TDbContext, TResult>(
        this DbContext context, ExecutionStrategyNext<TDbContext, TResult> operation) where TDbContext : DbContext
    {
        return new ExecutionStrategyOptions<TDbContext, TResult>(
            context.GetData(),
            context.GetMiddlewares<TDbContext, TResult>(),
            operation,
            default,
            null);
    }

    private static DependenciesContainer GetContainer(this DbContext context)
    {
        return WrapGetServiceCall(context.GetService<DependenciesContainer>);
    }

    private static TReturn WrapGetServiceCall<TReturn>(Func<TReturn> action)
    {
        try
        {
            return action();
        }
        catch (InvalidOperationException)
        {
            throw new ServicesForExecutionStrategyExtensionsNotFound(
                "Can't get required services for ExecutionStrategy.Extensions from DbContext. It's " +
                "likely due to you not calling builder.UseExecutionStrategyExtensions on your DbContext options builder.");
        }
    }
}