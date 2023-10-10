using EntityFrameworkCore.ExecutionStrategy.Extensions;
using EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;
using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using ExecutionStrategy.Extensions.IntegrationTests.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VerifyTests.EntityFramework;

namespace ExecutionStrategy.Extensions.IntegrationTests;

[FixtureService(Lifetime = ServiceLifetime.Transient)]
public class TestFixture : IDisposable, IServiceProvider
{
    public ServiceProvider RootServiceProvider { get; set; }
    public IServiceScope Scope { get; set; }
    public IServiceProvider Services => Scope.ServiceProvider;

    public TestFixture(IDbInfrastructure db)
    {
        var services = new ServiceCollection();
        ConfigureServices(services, db);
        RootServiceProvider = services.BuildServiceProvider();

        Scope = RootServiceProvider.CreateScope();
    }

    private void ConfigureServices(IServiceCollection services, IDbInfrastructure db)
    {
        services.AddSingleton(db);
        services.AddSingleton(provider => provider.GetRequiredService<IDbInfrastructure>().ProvideIsolatedInfrastructure());
    }

    public void Dispose()
    {
        RootServiceProvider.Dispose();
    }

    public object? GetService(Type serviceType)
    {
        return Services.GetService(serviceType);
    }

    public void ConfigureDbContext(DbContextOptionsBuilder builder)
    {
        Services.GetRequiredService<IIsolatedDbInfrastructure>().ConfigureDbContext(builder);
    }

    public AppDbContext CreateDbContext(Action<DbContextOptionsBuilder>? action = null)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        ConfigureDbContext(builder);
        action?.Invoke(builder);
        return new AppDbContext(builder.Options);
    }

    public AppDbContext CreateDbContextWithEmptyUse(Action<IExecutionStrategyPrimaryOptionsBuilder<AppDbContext>>? action = null)
    {
        action ??= (_) => { };

        return CreateDbContext(builder =>
        {
            builder.UseExecutionStrategyExtensions(action);
        });
    }

    public AppDbContext CreateDbContextWithClearChangeTracker()
    {
        return CreateDbContext(builder =>
        {
            builder.UseExecutionStrategyExtensions(builder => builder.WithClearChangeTrackerOnRetry());
        });
    }
}