using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public record ExecutionStrategyOperationArgs<TDbContext> : IExecutionStrategyOperationArgs<TDbContext>
    where TDbContext : DbContext
{
    public IExecutionStrategyData Data { get; set; }
    public TDbContext Context { get; set; }
    public int Attempt { get; set; }
    public CancellationToken CancellationToken { get; set; }

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