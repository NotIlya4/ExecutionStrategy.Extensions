using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public interface IActualDbContextProvider<out TDbContext> where TDbContext : DbContext
{
    TDbContext DbContext { get; }
    void OnNewContextAssigned(Action<TDbContext> action);
}