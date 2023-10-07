using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.UnitTests.DbContextConfigurator;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}