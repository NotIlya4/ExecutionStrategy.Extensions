using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

internal interface IDbContextRetrier<TDbContext> where TDbContext : DbContext
{
    IExecutionStrategy CreateExecutionStrategy();
    Task<TDbContext> ProvideDbContextForRetry(int attempt);
}