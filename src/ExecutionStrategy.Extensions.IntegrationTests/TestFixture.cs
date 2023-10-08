using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests;

public class TestFixture : IDisposable, IServiceProvider
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
    }

    public void RunBetweenTests()
    {
        Scope.Dispose();
        Scope = RootServiceProvider.CreateScope();
    }

    public void Dispose()
    {
        Scope.Dispose();
    }

    public object? GetService(Type serviceType)
    {
        return Services.GetService(serviceType);
    }
}

public interface ITestLifetime
{
    public Task OnTestStart();
    public Task OnTestFinish();
}