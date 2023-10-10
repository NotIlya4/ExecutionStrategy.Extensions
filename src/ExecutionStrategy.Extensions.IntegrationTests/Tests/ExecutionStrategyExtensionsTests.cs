using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;

namespace ExecutionStrategy.Extensions.IntegrationTests.Tests;

public class CoreExtensionsTests
{
    private readonly TestFixture _fixture;
    
    public CoreExtensionsTests(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void ExecuteExtendedAsync_EmptyMiddlewares_OperationInvoked()
    {
        
    }
}