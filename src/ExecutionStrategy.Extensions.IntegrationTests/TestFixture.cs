using EntityFrameworkCore.ExecutionStrategy.Extensions;
using EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;
using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using ExecutionStrategy.Extensions.IntegrationTests.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VerifyTests.EntityFramework;

namespace ExecutionStrategy.Extensions.IntegrationTests;

[FixtureLifetime]
public class TestFixture : IDisposable, IServiceProvider, ITestLifetime
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
        services.AddScoped<IIsolatedDbInfrastructure>(provider => provider.GetRequiredService<IDbInfrastructure>().ProvideIsolatedInfrastructure());
    }

    public void Dispose()
    {
        RootServiceProvider.Dispose();
    }

    public object? GetService(Type serviceType)
    {
        return Services.GetService(serviceType);
    }

    public Task OnTestStart()
    {
        Scope.Dispose();
        Scope = RootServiceProvider.CreateScope();
        return Task.CompletedTask;
    }

    public Task OnTestFinish()
    {
        return Task.CompletedTask;
    }

    public void ConfigureDbContext(DbContextOptionsBuilder builder)
    {
        Services.GetRequiredService<IIsolatedDbInfrastructure>().ConfigureDbContext(builder);
    }

    public AppDbContext CreateContext(Action<DbContextOptionsBuilder>? action = null)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        ConfigureDbContext(builder);
        builder.EnableRecording();
        action?.Invoke(builder);
        return new AppDbContext(builder.Options);
    }

    public AppDbContext CreateWithClearChangeTracker()
    {
        return CreateContext(builder =>
        {
            builder.UseExecutionStrategyExtensions(builder => builder.WithClearChangeTrackerOnRetry());
        });
    }
}