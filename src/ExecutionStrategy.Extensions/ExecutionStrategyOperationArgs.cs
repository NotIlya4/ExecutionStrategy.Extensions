using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public class ExecutionStrategyOperationArgs<TDbContext> : IExecutionStrategyOperationArgs<TDbContext>
    where TDbContext : DbContext
{
    public IExecutionStrategyData Data { get; }
    public TDbContext Context { get; }
    public int Attempt { get; }
    public CancellationToken CancellationToken { get; }

    public ExecutionStrategyOperationArgs(IExecutionStrategyData data, TDbContext context, int attempt,
        CancellationToken cancellationToken)
    {
        Data = data;
        Context = context;
        Attempt = attempt;
        CancellationToken = cancellationToken;
    }
}

public interface IExecutionStrategyOperationArgs<out TDbContext> where TDbContext : DbContext
{
    public TDbContext Context { get; }
    public int Attempt { get; }
    public CancellationToken CancellationToken { get; }
    public IExecutionStrategyData Data { get; }
}