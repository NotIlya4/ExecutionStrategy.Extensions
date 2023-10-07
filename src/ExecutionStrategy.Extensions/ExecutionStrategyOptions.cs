using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public record ExecutionStrategyOptions<TDbContext, TResult> : IExecutionStrategyOptions<TDbContext, TResult>
    where TDbContext : DbContext
{
    public IExecutionStrategyData Data { get; set; }
    public List<ExecutionStrategyMiddleware<TDbContext, TResult>> Middlewares { get; set; }
    public ExecutionStrategyOperation<TDbContext, TResult> Operation { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public ExecutionStrategyOperation<TDbContext, ExecutionResult<TResult>>? VerifySucceeded { get; set; }

    public ExecutionStrategyOptions(
        IExecutionStrategyData data,
        List<ExecutionStrategyMiddleware<TDbContext, TResult>> middlewares,
        ExecutionStrategyOperation<TDbContext, TResult> operation,
        CancellationToken cancellationToken,
        ExecutionStrategyOperation<TDbContext, ExecutionResult<TResult>>? verifySucceeded)
    {
        Data = data;
        Middlewares = middlewares;
        Operation = operation;
        CancellationToken = cancellationToken;
        VerifySucceeded = verifySucceeded;
    }
}

public interface IExecutionStrategyOptions<TDbContext, TResult> where TDbContext : DbContext
{
    public IExecutionStrategyData Data { get; set; }
    public List<ExecutionStrategyMiddleware<TDbContext, TResult>> Middlewares { get; set; }
    public ExecutionStrategyOperation<TDbContext, TResult> Operation { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public ExecutionStrategyOperation<TDbContext, ExecutionResult<TResult>>? VerifySucceeded { get; set; }
}