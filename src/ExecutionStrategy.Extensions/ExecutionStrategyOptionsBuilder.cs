using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

class ExecutionStrategyOptionsBuilder<TDbContext, TResult> : IExecutionStrategyOptionsBuilder<TDbContext, TResult>
    where TDbContext : DbContext
{
    public IExecutionStrategyOptions<TDbContext, TResult> Options { get; set; }

    public ExecutionStrategyOptionsBuilder(IExecutionStrategyOptions<TDbContext, TResult> options)
    {
        Options = options;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithOperation(
        ExecutionStrategyOperation<TDbContext, Task<TResult>> action)
    {
        Options.Operation = action;
        return this;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithVerifySucceeded(
        ExecutionStrategyOperation<TDbContext, Task<ExecutionResult<TResult>>> action)
    {
        Options.VerifySucceeded = action;
        return this;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithCancellationToken(CancellationToken token)
    {
        Options.CancellationToken = token;
        return this;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithTransaction(IsolationLevel isolationLevel)
    {
        Options.Data.Set(isolationLevel);
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

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithMiddleware(
        ExecutionStrategyMiddleware<TDbContext, Task<TResult>> middleware)
    {
        Options.Middlewares.Add(middleware);
        return this;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithData(Action<IExecutionStrategyData> action)
    {
        action(Options.Data);
        return this;
    }
}

public interface IExecutionStrategyOptionsBuilder<TDbContext, TResult> where TDbContext : DbContext
{
    public IExecutionStrategyOptions<TDbContext, TResult> Options { get; set; }
    
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithOperation(
        ExecutionStrategyOperation<TDbContext, Task<TResult>> action);
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithVerifySucceeded(
        ExecutionStrategyOperation<TDbContext, Task<ExecutionResult<TResult>>> action);
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithCancellationToken(CancellationToken token);
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithTransaction(IsolationLevel isolationLevel);
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithClearChangeTrackerOnRetry();
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithMiddleware(
        ExecutionStrategyMiddleware<TDbContext, Task<TResult>> middleware);
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithData(Action<IExecutionStrategyData> action);
}