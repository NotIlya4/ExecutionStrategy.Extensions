namespace EntityFrameworkCore.ExecutionStrategyExtended;

public enum DbContextRetrierType
{
    UseSame,
    ClearChangeTracker,
    CreateNew
}