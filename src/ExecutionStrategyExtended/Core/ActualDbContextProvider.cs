using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public class ActualDbContextProvider<TDbContext> : IActualDbContextProvider<TDbContext> where TDbContext : DbContext
{
    private TDbContext _dbContext;
    private event Action<TDbContext> InternalNewContextAssigned = _ => { }; 

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
            InternalNewContextAssigned(value);
        }
    }

    public ActualDbContextProvider(TDbContext dbContext)
    {
        _dbContext = dbContext;
        NewContextAssigned += _ => { };
    }

    public event Action<TDbContext> NewContextAssigned
    {
        add
        {
            value(_dbContext);
            InternalNewContextAssigned += value;
        }
        remove => InternalNewContextAssigned -= value;
    }
}