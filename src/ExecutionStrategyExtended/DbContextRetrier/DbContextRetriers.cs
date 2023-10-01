namespace EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;

internal static class DbContextRetriers
{
    public static DbContextRetrierConfiguration NewDbContextRetrier(bool disposePreviousContext = true)
    {
        return new DbContextRetrierConfiguration()
        {
            DbContextRetrierType = DbContextRetrierType.CreateNew,
            DisposePreviousContext = disposePreviousContext
        };
    }

    public static DbContextRetrierConfiguration ClearChangeTrackerRetrier()
    {
        return new DbContextRetrierConfiguration()
        {
            DbContextRetrierType = DbContextRetrierType.ClearChangeTracker
        };
    }
    
    public static DbContextRetrierConfiguration UseSameDbContextRetrier()
    {
        return new DbContextRetrierConfiguration()
        {
            DbContextRetrierType = DbContextRetrierType.UseSame
        };
    }
}