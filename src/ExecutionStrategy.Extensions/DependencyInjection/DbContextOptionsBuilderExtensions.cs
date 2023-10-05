using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder UseExecutionStrategyExtensions(this DbContextOptionsBuilder builder)
    {
        
    } 
}

internal class ExecutionStrategyExtensionsExtension<TDbContext, TResult> : IDbContextOptionsExtension where TDbContext : DbContext
{
    public ExecutionStrategyExtensionsExtension(ExecutionStrategyOptions<TDbContext, TResult> options)
    {
        
    }
    
    public void ApplyServices(IServiceCollection services)
    {
        
    }

    public void Validate(IDbContextOptions options)
    {
        
    }

    public DbContextOptionsExtensionInfo Info { get; } = null!;
}