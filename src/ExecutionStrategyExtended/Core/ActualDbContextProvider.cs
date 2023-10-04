using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public class ActualDbContextProvider<TDbContext> : IActualDbContextProvider<TDbContext> where TDbContext : DbContext
{
    private TDbContext _dbContext;
    private event EventHandler<TDbContext>? InternalNewContextAssigned; 

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
            InternalNewContextAssigned?.Invoke(this, value);
        }
    }

    public ActualDbContextProvider(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public event EventHandler<TDbContext> NewContextAssigned
    {
        add
        {
            value(this, _dbContext);
            InternalNewContextAssigned += value;
        }
        remove => InternalNewContextAssigned -= value;
    }
}