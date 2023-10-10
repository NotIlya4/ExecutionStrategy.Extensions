using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public record ExecutionStrategyOptions<TDbContext, TResult> : IExecutionStrategyOptions<TDbContext, TResult>
    where TDbContext : DbContext
{
    public IExecutionStrategyData Data { get; set; }
    public List<ExecutionStrategyMiddleware<TDbContext, TResult>> Middlewares { get; set; }
    public ExecutionStrategyNext<TDbContext, TResult> Operation { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public ExecutionStrategyNext<TDbContext, ExecutionResult<TResult>>? VerifySucceeded { get; set; }

    public ExecutionStrategyOptions(
        IExecutionStrategyData data,
        List<ExecutionStrategyMiddleware<TDbContext, TResult>> middlewares,
        ExecutionStrategyNext<TDbContext, TResult> operation,
        CancellationToken cancellationToken,
        ExecutionStrategyNext<TDbContext, ExecutionResult<TResult>>? verifySucceeded)
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
    public ExecutionStrategyNext<TDbContext, TResult> Operation { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public ExecutionStrategyNext<TDbContext, ExecutionResult<TResult>>? VerifySucceeded { get; set; }
}