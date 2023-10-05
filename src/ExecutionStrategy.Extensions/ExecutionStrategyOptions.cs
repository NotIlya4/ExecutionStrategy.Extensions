using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public class ExecutionStrategyOptions<TDbContext, TResult> : IExecutionStrategyOptions<TDbContext, TResult>
    where TDbContext : DbContext
{
    public IExecutionStrategyData Data { get; set; } = new ExecutionStrategyData();

    public List<ExecutionStrategyMiddleware<TDbContext, Task<TResult>>> Middlewares
    {
        get => Data.Get<List<ExecutionStrategyMiddleware<TDbContext, Task<TResult>>>>();
        set => Data.Set(value);
    }

    public ExecutionStrategyOperation<TDbContext, Task<TResult>> Operation
    {
        get => Data.Get<ExecutionStrategyOperation<TDbContext, Task<TResult>>>();
        set => Data.Set(value);
    }

    public CancellationToken CancellationToken
    {
        get => Data.Get<CancellationToken>(); 
        set => Data.Set(value);
    }

    public ExecutionStrategyOperation<TDbContext, Task<ExecutionResult<TResult>>>? VerifySucceeded
    {
        get => Data.TryGet<ExecutionStrategyOperation<TDbContext, Task<ExecutionResult<TResult>>>>();
        set => Data.Set(value);
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