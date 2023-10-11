using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using ExecutionStrategy.Extensions.IntegrationTests.Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public class DbInfrastructureProvider : IFixtureServicesProvider
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton((_) => new DbInfrastructureBuilder<AppDbContext>().UsePostgresLocalContainer());
    }
}