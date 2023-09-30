namespace EntityFrameworkCore.ExecutionStrategyExtended;

public class DbContextRetrierConfiguration
{
    internal DbContextRetrierConfiguration()
    {
        
    }
    
    public DbContextRetrierType DbContextRetrierType { get; set; }
    public bool DisposePreviousContext { get; set; }
}