using EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

internal static class DbContextCrudExtensions
{
    public static List<ExecutionStrategyMiddleware<TDbContext, TResult>> GetMiddlewares<TDbContext, TResult>(this DbContext context) where TDbContext : DbContext
    {
        return context.GetService<ExecutionStrategyContainer>().MiddlewaresManager
            .CastMiddlewares<TDbContext, TResult>().ToList();
    }
}