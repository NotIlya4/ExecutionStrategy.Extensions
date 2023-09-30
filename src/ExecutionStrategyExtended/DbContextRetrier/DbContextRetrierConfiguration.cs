namespace EntityFrameworkCore.ExecutionStrategyExtended;

public class DbContextRetrierConfiguration
{
    public DbContextRetrierType DbContextRetrierType { get; set; }
    public bool DisposePreviousContext { get; set; }
}