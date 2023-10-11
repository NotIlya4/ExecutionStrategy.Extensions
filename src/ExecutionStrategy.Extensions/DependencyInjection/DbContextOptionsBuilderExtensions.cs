using EntityFrameworkCore.ExecutionStrategy.Extensions.Container;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;

/// <summary>
/// <see cref="DbContextOptionsBuilder"/> extensions.
/// </summary>
public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Registers required services inside <see cref="TDbContext"/> and allows you to add some default options that will be applied on each ExecuteExtendedAsync call.
    /// </summary>
    /// <param name="builder">Builder.</param>
    /// <param name="action">Options builder action.</param>
    /// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
    /// <returns>Builder.</returns>
    public static DbContextOptionsBuilder UseExecutionStrategyExtensions<TDbContext>(
        this DbContextOptionsBuilder builder,
        Action<IExecutionStrategyPrimaryOptionsBuilder<TDbContext>>? action = null) where TDbContext : DbContext
    {
        var container = new DependenciesContainer(new MiddlewaresManager(), new ExecutionStrategyData());
        action?.Invoke(new ExecutionStrategyPrimaryOptionsBuilder<TDbContext>(container));

        ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(
            new ExecutionStrategyEfExtension(container));

        return builder;
    }
    
    /// <summary>
    /// Registers required services inside <see cref="DbContext"/> and allows you to add some default options that will be applied on each ExecuteExtendedAsync call.
    /// </summary>
    /// <param name="builder">Builder.</param>
    /// <param name="action">Options builder action.</param>
    /// <returns>Builder.</returns>
    public static DbContextOptionsBuilder UseExecutionStrategyExtensions(
        this DbContextOptionsBuilder builder,
        Action<IExecutionStrategyPrimaryOptionsBuilder<DbContext>>? action = null)
    {
        return builder.UseExecutionStrategyExtensions<DbContext>(action);
    }
}