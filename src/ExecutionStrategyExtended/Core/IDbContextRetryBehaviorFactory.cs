using EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public interface IDbContextRetryBehaviorFactory<TDbContext> where TDbContext : DbContext
{
    IDbContextRetryBehavior<TDbContext> Create(TDbContext mainContext);
}