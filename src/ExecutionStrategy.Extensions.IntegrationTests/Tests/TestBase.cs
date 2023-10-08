using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests.Tests;

[UsesVerify]
public class TestBase
{
    private readonly TestFixture _fixture;

    public TestBase(TestFixture fixture)
    {
        _fixture = fixture;
        fixture.RunBetweenTests();
    }
}