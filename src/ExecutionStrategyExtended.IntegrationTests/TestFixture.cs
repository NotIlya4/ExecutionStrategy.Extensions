using EntityFrameworkCore.ExecutionStrategyExtended.DependencyInjection;
using ExecutionStrategyExtended.UnitTests.DbContextConfigurator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategyExtended.UnitTests;

public class TestFixture
{
    public IServiceProvider Services { get; }
    
    public TestFixture()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContextFactory<AppDbContext>(builder =>
        {
            builder.UseSqlite("Data Source=:memory:");
        });
        serviceCollection.AddExecutionStrategyExtended<AppDbContext>();

        Services = serviceCollection.BuildServiceProvider();
    }
}