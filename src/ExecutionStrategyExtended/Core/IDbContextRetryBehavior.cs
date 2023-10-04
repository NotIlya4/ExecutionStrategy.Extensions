using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public interface IDbContextRetryBehavior<TDbContext> where TDbContext : DbContext
{
    Task<TDbContext> ProvideDbContextForRetry(int attempt);
}