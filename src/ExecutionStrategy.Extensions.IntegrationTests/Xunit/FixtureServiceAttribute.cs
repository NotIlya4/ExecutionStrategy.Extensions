using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests.Xunit;

public class FixtureServiceAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;
    public Type? ServiceType { get; set; }
    public Func<IServiceProvider, object>? Factory { get; set; }
}