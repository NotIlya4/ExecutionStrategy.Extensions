using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <inheritdoc />
public record ExecutionStrategyOptions<TDbContext, TResult>(
    IExecutionStrategyData Data,
    List<ExecutionStrategyMiddleware<TDbContext, TResult>> Middlewares,
    ExecutionStrategyNext<TDbContext, TResult> Operation,
    CancellationToken CancellationToken,
    ExecutionStrategyNext<TDbContext, ExecutionResult<TResult>>? VerifySucceeded) : IExecutionStrategyOptions<TDbContext, TResult>
    where TDbContext : DbContext
{
    /// <inheritdoc />
    public IExecutionStrategyData Data { get; set; } = Data;

    /// <inheritdoc />
    public List<ExecutionStrategyMiddleware<TDbContext, TResult>> Middlewares { get; set; } = Middlewares;

    /// <inheritdoc />
    public ExecutionStrategyNext<TDbContext, TResult> Operation { get; set; } = Operation;

    /// <inheritdoc />
    public CancellationToken CancellationToken { get; set; } = CancellationToken;

    /// <inheritdoc />
    public ExecutionStrategyNext<TDbContext, ExecutionResult<TResult>>? VerifySucceeded { get; set; } = VerifySucceeded;
}

/// <summary>
/// Options for core extension method.
/// </summary>
/// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
/// <typeparam name="TResult">Return type of <see cref="IExecutionStrategyOptions{TDbContext,TResult}.Operation"/>.</typeparam>
public interface IExecutionStrategyOptions<TDbContext, TResult> where TDbContext : DbContext
{
    /// <summary>
    /// Custom data that can be accessed anytime.
    /// </summary>
    public IExecutionStrategyData Data { get; set; }
    
    /// <summary>
    /// Middlewares that will be called in descending order. Last middleware called first.
    /// </summary>
    public List<ExecutionStrategyMiddleware<TDbContext, TResult>> Middlewares { get; set; }
    
    /// <summary>
    /// Operation that will be executed. Any transient exception will lead to retry of operation.
    /// </summary>
    public ExecutionStrategyNext<TDbContext, TResult> Operation { get; set; }
    
    /// <summary>
    /// Cancellation token.
    /// </summary>
    public CancellationToken CancellationToken { get; set; }
    
    /// <summary>
    /// Verify succeeded that will be passed to <see cref="IExecutionStrategy"/>.
    /// </summary>
    public ExecutionStrategyNext<TDbContext, ExecutionResult<TResult>>? VerifySucceeded { get; set; }
}