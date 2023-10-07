using ExecutionStrategy.Extensions.Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategy.Extensions.Demo.EntityFramework;

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