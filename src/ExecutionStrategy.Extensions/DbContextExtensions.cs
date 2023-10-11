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
    /// <param name="context"><see cref="TDbContext"/> instance</param>
    /// <param name="operation">Operation</param>
    /// <param name="action">Action builder</param>
    /// <typeparam name="TDbContext">Type of your DbContext</typeparam>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <returns>Operation result</returns>
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
    /// <inheritdoc cref="ExecuteExtendedAsync{TDbContext,TResult}(TDbContext,ExecutionStrategyNext{TDbContext,TResult},Action{IExecutionStrategyOptionsBuilder{TDbContext,TResult}}?)"/>
    /// </summary>
    /// <param name="context"><see cref="TDbContext"/> instance</param>
    /// <param name="operation">Operation</param>
    /// <param name="action">Action builder</param>
    /// <typeparam name="TDbContext">Type of your DbContext</typeparam>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <returns>Operation result</returns>
    public static Task<TResult> ExecuteExtendedAsync<TDbContext, TResult>(this TDbContext context,
        ExecutionStrategyNext<TResult> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, TResult>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(_ => operation(), action);
    }
    
    /// <summary>
    /// <inheritdoc cref="ExecuteExtendedAsync{TDbContext,TResult}(TDbContext,ExecutionStrategyNext{TDbContext,TResult},Action{IExecutionStrategyOptionsBuilder{TDbContext,TResult}}?)"/>
    /// </summary>
    /// <param name="context"><see cref="TDbContext"/> instance</param>
    /// <param name="operation">Operation</param>
    /// <param name="action">Action builder</param>
    /// <typeparam name="TDbContext">Type of your DbContext</typeparam>
    /// <returns>Operation result</returns>
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
    /// <inheritdoc cref="ExecuteExtendedAsync{TDbContext,TResult}(TDbContext,ExecutionStrategyNext{TDbContext,TResult},Action{IExecutionStrategyOptionsBuilder{TDbContext,TResult}}?)"/>
    /// </summary>
    /// <param name="context"><see cref="TDbContext"/> instance</param>
    /// <param name="operation">Operation</param>
    /// <param name="action">Action builder</param>
    /// <typeparam name="TDbContext">Type of your DbContext</typeparam>
    /// <returns>Operation result</returns>
    public static Task ExecuteExtendedAsync<TDbContext>(this TDbContext context,
        ExecutionStrategyVoidNext operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, Void>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(_ => operation(), action);
    }
}