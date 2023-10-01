namespace EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier;

internal class DbContextRetrierConfiguration
{
    internal DbContextRetrierConfiguration()
    {
        
    }
    
    public DbContextRetrierType DbContextRetrierType { get; set; }
    public bool DisposePreviousContext { get; set; }
}