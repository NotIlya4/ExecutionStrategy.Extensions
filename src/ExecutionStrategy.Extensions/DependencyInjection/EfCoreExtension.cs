using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;

internal class EfCoreExtension: IDbContextOptionsExtension
{
    private readonly DependencyContainer _container;

    public EfCoreExtension(DependencyContainer container)
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