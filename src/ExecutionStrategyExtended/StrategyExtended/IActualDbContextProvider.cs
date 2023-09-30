using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

public interface IActualDbContextProvider<out TDbContext> where TDbContext : DbContext
{
    TDbContext DbContext { get; }
}