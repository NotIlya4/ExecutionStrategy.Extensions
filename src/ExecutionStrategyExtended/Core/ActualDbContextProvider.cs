using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

internal class ActualDbContextProvider<TDbContext> : IActualDbContextProvider<TDbContext> where TDbContext : DbContext
{
    private readonly List<Action<TDbContext>> _subscribers = new();
    private TDbContext _dbContext;

    public TDbContext DbContext
    {
        get => _dbContext;
        set
        {
            if (ReferenceEquals(_dbContext, value))
            {
                return;
            }
            
            _dbContext = value;
            _subscribers.ForEach(action => action(value));
        }
    }

    public ActualDbContextProvider(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void OnNewContextAssigned(Action<TDbContext> action)
    {
        _subscribers.Add(action);
        action(DbContext);
    }
}