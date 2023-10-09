using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;

namespace ExecutionStrategy.Extensions.IntegrationTests.Tests;

public class CoreExtensionsTests
{
    private readonly AppDbContext _context;
    
    public CoreExtensionsTests(TestFixture fixture)
    {
        _context = fixture.AppDbContext();
    }
}