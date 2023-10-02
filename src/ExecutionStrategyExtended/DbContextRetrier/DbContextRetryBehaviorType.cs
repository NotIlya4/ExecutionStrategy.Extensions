namespace EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;

internal enum DbContextRetryBehaviorType
{
    UseSame,
    ClearChangeTracker,
    CreateNew
}