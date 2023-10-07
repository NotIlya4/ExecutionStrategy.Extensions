using EntityFrameworkCore.ExecutionStrategy.Extensions;
using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests.Tests;

[Collection("default")]
public class UnitTest1
{
    private readonly IDbBootstrapper _bootstrapper;
    private readonly AppDbContext _context;
    
    public UnitTest1(TestFixture fixture)
    {
        _bootstrapper = fixture.Bootstrapper;
        _context = fixture.Services.GetRequiredService<AppDbContext>();

        _bootstrapper.Clean();
    }
    
    [Fact]
    public async Task Test1()
    {
        await _context.ExecuteExtendedAsync(async () =>
        {
            _context.Users.Add(new User(0, "Biba", false));
            await _context.SaveChangesAsync();
        });
        
        _context.Clear();

        Verify(await _context.Users.SingleAsync());
    }
}