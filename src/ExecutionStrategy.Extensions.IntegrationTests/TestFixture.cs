using ExecutionStrategyExtended.UnitTests.DbContextConfigurator;
using ExecutionStrategyExtended.UnitTests.PostgresBootstrapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategyExtended.UnitTests;

public class TestFixture
{
    public IServiceScope Scope { get; set; }
    public IServiceProvider Services => Scope.ServiceProvider;
    public IDbBootstrapper Bootstrapper => Services.GetRequiredService<IDbBootstrapper>();
    
    
    public TestFixture()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<IDbBootstrapper>();
        serviceCollection.AddAppDbContext(new SqliteDbContextOptions());

        Scope = serviceCollection.BuildServiceProvider().CreateScope();
    }
}