namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

internal enum DbContextRetryBehaviorType
{
    UseSame,
    ClearChangeTracker,
    CreateNew
}