using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests.Xunit;

public interface IFixtureServicesProvider
{
    void ConfigureServices(IServiceCollection services);
}