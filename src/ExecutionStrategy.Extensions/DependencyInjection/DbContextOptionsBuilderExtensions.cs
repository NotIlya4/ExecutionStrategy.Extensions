using EntityFrameworkCore.ExecutionStrategy.Extensions.Container;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseExecutionStrategyExtensions<TDbContext>(
        this DbContextOptionsBuilder builder,
        Action<IExecutionStrategyPrimaryOptionsBuilder<TDbContext>>? action = null) where TDbContext : DbContext
    {
        var container = new DependencyContainer(new MiddlewaresManager(), new ExecutionStrategyData());
        action?.Invoke(new ExecutionStrategyPrimaryOptionsBuilder<TDbContext>(container));

        ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(
            new EfCoreExtension(container));

        return builder;
    }
    
    public static DbContextOptionsBuilder UseExecutionStrategyExtensions(
        this DbContextOptionsBuilder builder,
        Action<IExecutionStrategyPrimaryOptionsBuilder<DbContext>>? action = null)
    {
        return builder.UseExecutionStrategyExtensions<DbContext>(action);
    }
}