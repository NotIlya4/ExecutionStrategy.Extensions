namespace EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;

internal static class DbContextRetryBehaviors
{
    public static DbContextRetryBehaviorOptions CreateNewDbContextRetrier(bool disposePreviousContext = true)
    {
        return new DbContextRetryBehaviorOptions()
        {
            DbContextRetryBehaviorType = DbContextRetryBehaviorType.CreateNew,
            DisposePreviousContext = disposePreviousContext
        };
    }

    public static DbContextRetryBehaviorOptions ClearChangeTrackerRetrier()
    {
        return new DbContextRetryBehaviorOptions()
        {
            DbContextRetryBehaviorType = DbContextRetryBehaviorType.ClearChangeTracker
        };
    }
    
    public static DbContextRetryBehaviorOptions UseSameDbContextRetrier()
    {
        return new DbContextRetryBehaviorOptions()
        {
            DbContextRetryBehaviorType = DbContextRetryBehaviorType.UseSame
        };
    }
}