namespace ExecutionStrategy.Extensions.IntegrationTests.Tests;

[UsesVerify]
public class TestBase : IClassFixture<TestFixture>
{
    private readonly TestFixture _fixture;

    public TestBase(TestFixture fixture)
    {
        _fixture = fixture;
        fixture.RunBetweenTests();
    }
}