namespace EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;

internal enum DbContextRetrierType
{
    UseSame,
    ClearChangeTracker,
    CreateNew
}