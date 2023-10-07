using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests;

[CollectionDefinition("default")]
public class TestFixture : ICollectionFixture<TestFixture>, IDisposable
{
    public IServiceScope Scope { get; set; }
    public IServiceProvider Services => Scope.ServiceProvider;
    public IDbBootstrapper Bootstrapper => Services.GetRequiredService<IDbBootstrapper>();

    public TestFixture()
    {
        var services = new ServiceCollection();
        
        var db = DbInfrastructureThemes.CreateForSqlite();
        services.ApplyDbInfrastructure(db);
        
        Scope = services.BuildServiceProvider().CreateScope();

        Bootstrapper.Bootstrap();
    }

    public void Dispose()
    {
        Bootstrapper.Destroy();
        Scope.Dispose();
    }
}