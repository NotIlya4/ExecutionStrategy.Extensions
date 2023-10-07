using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbContextConfigurator;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
}