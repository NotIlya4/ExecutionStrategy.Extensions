using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;

internal class ExecutionStrategyExtensionsExtension: IDbContextOptionsExtension
{
    private readonly ExecutionStrategyContainer _container;

    public ExecutionStrategyExtensionsExtension(ExecutionStrategyContainer container)
    {
        _container = container;
    }

    public void ApplyServices(IServiceCollection services)
    {
        services.AddSingleton(_container);
    }

    public void Validate(IDbContextOptions options)
    {
    }

    public DbContextOptionsExtensionInfo Info => null!;
}