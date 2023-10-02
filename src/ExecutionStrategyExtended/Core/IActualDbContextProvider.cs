using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public interface IActualDbContextProvider<out TDbContext> where TDbContext : DbContext
{
    TDbContext DbContext { get; }
    event Action<TDbContext> NewContextAssigned;
}