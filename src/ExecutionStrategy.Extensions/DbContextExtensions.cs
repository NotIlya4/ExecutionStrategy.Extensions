using EntityFrameworkCore.ExecutionStrategy.Extensions.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <summary>
/// <see cref="IExecutionStrategy"/> extensions.
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Executes your <see cref="operation"/> wrapped with <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Middlewares"/> inside <see cref="IExecutionStrategy"/> and if any transient exception occured retries operation.
    /// </summary>
    /// <param name="context">Your context instance.</param>
    /// <param name="operation">Operation.</param>
    /// <param name="action">Options builder action.</param>
    /// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
    /// <typeparam name="TResult">Return type of <paramref name="operation"/>.</typeparam>
    /// <returns>Result from <paramref name="operation"/>.</returns>
    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(this TDbContext context,
        ExecutionStrategyNext<TDbContext, TResult> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, TResult>>? action = null) where TDbContext : DbContext
    {
        var options = context.CreateOptionsFromPrimary(operation);
        var builder = new ExecutionStrategyOptionsBuilder<TDbContext, TResult>(options);
        
        action?.Invoke(builder);

        return context.ExecuteExtendedAsync(options);
    }
    
    /// <summary>
    /// Executes your <see cref="operation"/> wrapped with <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Middlewares"/> inside <see cref="IExecutionStrategy"/> and if any transient exception occured retries operation.
    /// </summary>
    /// <param name="context">Your context instance.</param>
    /// <param name="operation">Operation.</param>
    /// <param name="action">Options builder action.</param>
    /// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
    /// <typeparam name="TResult">Return type of <paramref name="operation"/>.</typeparam>
    /// <returns>Result from <paramref name="operation"/>.</returns>
    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(this TDbContext context,
        ExecutionStrategyNext<TResult> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, TResult>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(_ => operation(), action);
    }
    
    /// <summary>
    /// Executes your <see cref="operation"/> wrapped with <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Middlewares"/> inside <see cref="IExecutionStrategy"/> and if any transient exception occured retries operation.
    /// </summary>
    /// <param name="context">Your context instance.</param>
    /// <param name="operation">Operation.</param>
    /// <param name="action">Options builder action.</param>
    /// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
    /// <returns>Result from <paramref name="operation"/>.</returns>
    public static Task ExecuteExtendedAsync<TDbContext>(this TDbContext context,
        ExecutionStrategyVoidNext<TDbContext> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, Void>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(async args =>
        {
            await operation(args);
            return Void.Instance;
        }, action);
    }
    
    /// <summary>
    /// Executes your <see cref="operation"/> wrapped with <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Middlewares"/> inside <see cref="IExecutionStrategy"/> and if any transient exception occured retries operation.
    /// </summary>
    /// <param name="context">Your context instance.</param>
    /// <param name="operation">Operation.</param>
    /// <param name="action">Options builder action.</param>
    /// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
    /// <returns>Result from <paramref name="operation"/>.</returns>
    public static Task ExecuteExtendedAsync<TDbContext>(this TDbContext context,
        ExecutionStrategyVoidNext operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, Void>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(_ => operation(), action);
    }
}