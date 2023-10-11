using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <inheritdoc />
public record ExecutionStrategyOperationArgs<TDbContext>(
    IExecutionStrategyData Data, 
    TDbContext Context, 
    int Attempt,
    CancellationToken CancellationToken) : IExecutionStrategyOperationArgs<TDbContext>
    where TDbContext : DbContext
{
    /// <inheritdoc />
    public IExecutionStrategyData Data { get; set; } = Data;

    /// <inheritdoc />
    public TDbContext Context { get; set; } = Context;

    /// <inheritdoc />
    public int Attempt { get; set; } = Attempt;

    /// <inheritdoc />
    public CancellationToken CancellationToken { get; set; } = CancellationToken;
}

/// <summary>
/// Operation args.
/// </summary>
/// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
public interface IExecutionStrategyOperationArgs<out TDbContext> where TDbContext : DbContext
{
    /// <summary>
    /// Context instance that can be overriden inside middleware.
    /// </summary>
    public TDbContext Context { get; }
    
    /// <summary>
    /// Retry attempt number.
    /// </summary>
    public int Attempt { get; }
    
    /// <summary>
    /// Cancellation token.
    /// </summary>
    public CancellationToken CancellationToken { get; }
    
    /// <summary>
    /// Custom data that can be accessed anytime.
    /// </summary>
    public IExecutionStrategyData Data { get; }
}