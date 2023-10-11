using EntityFrameworkCore.ExecutionStrategy.Extensions.Container;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;

/// <summary>
/// <see cref="DbContextOptionsBuilder"/> extensions for ExecutionStrategy.Extensions 
/// </summary>
public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    /// <inheritdoc cref="UseExecutionStrategyExtensions"/>
    /// </summary>
    /// <param name="builder"><inheritdoc cref="UseExecutionStrategyExtensions"/></param>
    /// <param name="action"><inheritdoc cref="UseExecutionStrategyExtensions"/></param>
    /// <typeparam name="TDbContext">Your DbContext type</typeparam>
    /// <returns></returns>
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
    /// Registers required services inside <see cref="DbContext"/> and allows you to add some default options that will be applied on each ExecuteExtendedAsync
    /// </summary>
    /// <param name="builder">Options builder</param>
    /// <param name="action">Builder action</param>
    /// <returns>Options builder</returns>
    public static DbContextOptionsBuilder UseExecutionStrategyExtensions(
        this DbContextOptionsBuilder builder,
        Action<IExecutionStrategyPrimaryOptionsBuilder<DbContext>>? action = null)
    {
        return builder.UseExecutionStrategyExtensions<DbContext>(action);
    }
}