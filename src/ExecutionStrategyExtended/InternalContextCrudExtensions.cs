using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

internal static class InternalContextCrudExtensions
{
    public static IDbContextRetryBehavior<TDbContext> GetRetryBehavior<TDbContext>(this TDbContext context) where TDbContext : DbContext
    {
        try
        {
            return context.GetService<IDbContextRetryBehavior<TDbContext>>();
        }
        catch (InvalidOperationException e)
        {
            throw WrapException(e);
        }
    }
    
    public static Exception WrapException(Exception e)
    {
        return e;
    }
}