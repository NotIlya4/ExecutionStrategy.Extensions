using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public record ExecutionStrategyOptions<TDbContext, TResult> : IExecutionStrategyOptions<TDbContext, TResult>
    where TDbContext : DbContext
{
    public IExecutionStrategyData Data { get; set; }
    public List<ExecutionStrategyMiddleware<TDbContext, Task<TResult>>> Middlewares { get; set; }
    public ExecutionStrategyOperation<TDbContext, Task<TResult>> Operation { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public ExecutionStrategyOperation<TDbContext, Task<ExecutionResult<TResult>>>? VerifySucceeded { get; set; }

    public ExecutionStrategyOptions(IExecutionStrategyData data,
        List<ExecutionStrategyMiddleware<TDbContext, Task<TResult>>> middlewares,
        ExecutionStrategyOperation<TDbContext, Task<TResult>> operation, CancellationToken cancellationToken,
        ExecutionStrategyOperation<TDbContext, Task<ExecutionResult<TResult>>>? verifySucceeded)
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
    public List<ExecutionStrategyMiddleware<TDbContext, Task<TResult>>> Middlewares { get; set; }
    public ExecutionStrategyOperation<TDbContext, Task<TResult>> Operation { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public ExecutionStrategyOperation<TDbContext, Task<ExecutionResult<TResult>>>? VerifySucceeded { get; set; }
}