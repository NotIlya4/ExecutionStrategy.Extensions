using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

class ExecutionStrategyOptionsBuilder<TDbContext, TResult> : IExecutionStrategyOptionsBuilder<TDbContext, TResult> where TDbContext : DbContext
{
    public IExecutionStrategyData Data { get; set; }

    public List<ExecutionStrategyMiddleware<TDbContext, Task<TResult>>> Middlewares
    {
        get => Data.Get<List<ExecutionStrategyMiddleware<TDbContext, Task<TResult>>>>(); 
        set => Data.Set(value);
    }

    public ExecutionStrategyOptionsBuilder() : this(new ExecutionStrategyOptions<TDbContext, TResult>())
    {
        
    }
    
    public ExecutionStrategyOptionsBuilder(IExecutionStrategyOptions<TDbContext, TResult> options)
    {
        Data = options.Data;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithOperation(ExecutionStrategyOperation<TDbContext, Task<TResult>> action)
    {
        Data.Set(action);
        return this;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithValidateSucceeded(ExecutionStrategyOperation<TDbContext, Task<ExecutionResult<TResult>>> action)
    {
        Data.Set(action);
        return this;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithCancellationToken(CancellationToken token)
    {
        Data.Set(token);
        return this;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithTransaction(IsolationLevel isolationLevel)
    {
        Data.Set(isolationLevel);
        return WithMiddleware(async (next, args) =>
        {
            await using var transaction = await args.Context.Database.BeginTransactionAsync(isolationLevel);
            args.Data.Set(transaction);
            return await next(args);
        });
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithClearChangeTrackerOnRetry()
    {
        return WithMiddleware(async (next, args) =>
        {
            args.Context.ChangeTracker.Clear();
            return await next(args);
        });
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithMiddleware(ExecutionStrategyMiddleware<TDbContext, Task<TResult>> middleware)
    {
        Middlewares.Add(middleware);
        return this;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithData(Action<IExecutionStrategyData> action)
    {
        action(Data);
        return this;
    }
}

public interface IExecutionStrategyOptionsBuilder<TDbContext, TResult> where TDbContext : DbContext
{
    public List<ExecutionStrategyMiddleware<TDbContext, Task<TResult>>> Middlewares { get; set; }
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithOperation(ExecutionStrategyOperation<TDbContext, Task<TResult>> action);
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithValidateSucceeded(ExecutionStrategyOperation<TDbContext, Task<ExecutionResult<TResult>>> action);
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithCancellationToken(CancellationToken token);
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithTransaction(IsolationLevel isolationLevel);
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithClearChangeTrackerOnRetry();
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithMiddleware(ExecutionStrategyMiddleware<TDbContext, Task<TResult>> middleware);
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithData(Action<IExecutionStrategyData> action);
}