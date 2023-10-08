using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests;

[CollectionDefinition("default")]
public class TestFixture : ICollectionFixture<TestFixture>, IDisposable
{
    public IServiceProvider RootServiceProvider { get; set; }
    public IServiceScope Scope { get; set; }
    public IServiceProvider Services => Scope.ServiceProvider;

    public TestFixture()
    {
        var services = new ServiceCollection();
        
        ConfigureServices(services);

        RootServiceProvider = services.BuildServiceProvider();
        Scope = RootServiceProvider.CreateScope();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.ApplyDbInfrastructure(new DbInfrastructureBuilder<AppDbContext>().UsePostgresLocalContainer());
    }

    public void RunBetweenTests()
    {
        Scope.Dispose();
        Scope = RootServiceProvider.CreateScope();
    }

    public void Dispose()
    {
        Scope.Dispose();
        RootServiceProvider.GetRequiredService<IDbInfrastructure>().Destroy();
    }
}