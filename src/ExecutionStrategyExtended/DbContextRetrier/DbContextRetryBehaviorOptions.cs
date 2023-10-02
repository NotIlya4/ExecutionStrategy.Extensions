namespace EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;

internal class DbContextRetryBehaviorOptions
{
    internal DbContextRetryBehaviorOptions()
    {
        
    }
    
    public DbContextRetryBehaviorType DbContextRetryBehaviorType { get; set; }
    public bool DisposePreviousContext { get; set; }
}