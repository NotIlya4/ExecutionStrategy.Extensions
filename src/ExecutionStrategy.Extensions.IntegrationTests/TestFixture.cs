using EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;
using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using ExecutionStrategy.Extensions.IntegrationTests.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests;

public class TestFixture : IDisposable, IServiceProvider, ITestLifetime
{
    public IServiceProvider RootServiceProvider { get; set; }
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
        services.ApplyDbInfrastructure(db);
        services.AddScoped<TransientExceptionThrower>();
    }

    public void Dispose()
    {
        Scope.Dispose();
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

    public AppDbContext CreateEmptyContext()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        Services.GetRequiredService<IIsolatedDbInfrastructure>().ConfigureDbContext(builder);
        builder.UseExecutionStrategyExtensions();
        return new AppDbContext(builder.Options);
    }
}

public interface ITestLifetime
{
    public Task OnTestStart();
    public Task OnTestFinish();
}