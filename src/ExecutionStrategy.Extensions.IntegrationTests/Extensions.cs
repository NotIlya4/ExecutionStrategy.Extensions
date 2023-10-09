using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategy.Extensions.IntegrationTests;

public static class Extensions
{
    public static void ClearTables(this AppDbContext context)
    {
        context.RemoveRange(context.Users.ToList());
        context.SaveChanges();
    }

    public static void ChangeTrackerClear(this AppDbContext context)
    {
        context.ChangeTracker.Clear();
    }

    public static void EnsureDeletedCreated(this DbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}