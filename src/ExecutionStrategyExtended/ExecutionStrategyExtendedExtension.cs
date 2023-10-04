using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

public class ExecutionStrategyExtendedExtension<TDbContext> : IDbContextOptionsExtension where TDbContext : DbContext
{
    public void ApplyServices(IServiceCollection services)
    {
        services.AddSingleton<ExecutionStrategyExtended<TDbContext>>();
    }

    public void Validate(IDbContextOptions options)
    {
        
    }

    public DbContextOptionsExtensionInfo Info { get; } = null!;
}