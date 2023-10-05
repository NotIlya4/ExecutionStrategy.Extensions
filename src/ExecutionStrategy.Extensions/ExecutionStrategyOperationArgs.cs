using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public struct ExecutionStrategyOperationArgs<TDbContext> : IExecutionStrategyOperationArgs<TDbContext>
    where TDbContext : DbContext
{
    public IExecutionStrategyData Data { get; set; } = new ExecutionStrategyData();

    public TDbContext Context
    {
        get => Data.Get<TDbContext>(); 
        set => Data.Set(value);
    }

    public int Attempt
    {
        get => Data.Get<int>("Attempt"); 
        set => Data["Attempt"] = value;
    }

    public CancellationToken CancellationToken
    {
        get => Data.Get<CancellationToken>(); 
        set => Data.Set(value);
    }

    public ExecutionStrategyOperationArgs()
    {
        
    }

    public ExecutionStrategyOperationArgs(IExecutionStrategyData data)
    {
        Data = data;
    }
}

public interface IExecutionStrategyOperationArgs<TDbContext> where TDbContext : DbContext
{
    public TDbContext Context { get; set; }
    public int Attempt { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public IExecutionStrategyData Data { get; set; }
}