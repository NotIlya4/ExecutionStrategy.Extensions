using ExecutionStrategyExtended.Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.Demo.EntityFramework;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}