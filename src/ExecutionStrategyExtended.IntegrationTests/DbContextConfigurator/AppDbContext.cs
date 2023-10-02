using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.UnitTests;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}