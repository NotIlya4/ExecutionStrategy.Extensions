using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public interface IActualDbContextProvider<TDbContext> where TDbContext : DbContext
{
    TDbContext DbContext { get; }
    event EventHandler<TDbContext> NewContextAssigned;
}