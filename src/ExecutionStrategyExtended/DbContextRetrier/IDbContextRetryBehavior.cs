using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;

internal interface IDbContextRetryBehavior<TDbContext> where TDbContext : DbContext
{
    IExecutionStrategy CreateExecutionStrategy();
    Task<TDbContext> ProvideDbContextForRetry(int attempt);
}