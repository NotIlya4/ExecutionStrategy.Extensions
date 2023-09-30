using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended;

public interface IActualDbContextProvider<out TDbContext> where TDbContext : DbContext
{
    TDbContext DbContext { get; }
}