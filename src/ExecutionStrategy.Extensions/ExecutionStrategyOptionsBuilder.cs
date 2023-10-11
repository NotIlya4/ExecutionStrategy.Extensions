using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

internal class ExecutionStrategyOptionsBuilder<TDbContext, TResult> : IExecutionStrategyOptionsBuilder<TDbContext, TResult>
    where TDbContext : DbContext
{
    public IExecutionStrategyOptions<TDbContext, TResult> Options { get; set; }
    public IExecutionStrategyData Data => Options.Data;
    public List<ExecutionStrategyMiddleware<TDbContext, TResult>> Middlewares => Options.Middlewares;
    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithMiddleware(ExecutionStrategyMiddleware<TDbContext, TResult> middleware)
    {
        Middlewares.Add(middleware);
        return this;
    }

    public ExecutionStrategyOptionsBuilder(IExecutionStrategyOptions<TDbContext, TResult> options)
    {
        Options = options;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithOperation(
        ExecutionStrategyNext<TDbContext, TResult> action)
    {
        Options.Operation = action;
        return this;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithVerifySucceeded(
        ExecutionStrategyNext<TDbContext, ExecutionResult<TResult>> verifySucceeded)
    {
        Options.VerifySucceeded = verifySucceeded;
        return this;
    }

    public IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithCancellationToken(CancellationToken token)
    {
        Options.CancellationToken = token;
        return this;
    }
}

/// <summary>
/// Options builder.
/// </summary>
/// <typeparam name="TDbContext">Type of your DbContext</typeparam>
/// <typeparam name="TResult">Type of result from your operation</typeparam>
public interface IExecutionStrategyOptionsBuilder<TDbContext, TResult> : 
    IBuilderWithMiddlewares<TDbContext, TResult, IExecutionStrategyOptionsBuilder<TDbContext, TResult>>, 
    IBuilderWithData, 
    IBuilderWithVerifySucceeded<TDbContext, TResult, IExecutionStrategyOptionsBuilder<TDbContext, TResult>> 
    where TDbContext : DbContext
{
    /// <summary>
    /// Provides cancellation token.
    /// </summary>
    /// <param name="token">Cancellation token.</param>
    /// <returns>Builder.</returns>
    IExecutionStrategyOptionsBuilder<TDbContext, TResult> WithCancellationToken(CancellationToken token);
}