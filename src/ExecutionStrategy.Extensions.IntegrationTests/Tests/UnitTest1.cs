using EntityFrameworkCore.ExecutionStrategy.Extensions;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests.Tests;

public class UnitTest1
{
    private readonly TestFixture _fixture;
    private readonly AppDbContext _context;
    private readonly TransientExceptionThrower _thrower;

    public UnitTest1(TestFixture fixture)
    {
        _fixture = fixture;
        _context = fixture.AppDbContext();
        _thrower = fixture.GetRequiredService<TransientExceptionThrower>();
    }
    
    [Fact]
    public async Task AddUserAndTransientExceptionOccured_OnSaveChangesSaveOnlyOneUser()
    {
        await _context.ExecuteExtendedAsync(async () =>
        {
            _context.Users.Add(new User(0, "Biba", false));

            _thrower.ThrowOnlyOnce();

            await _context.SaveChangesAsync();
        });

        _context.Clear();

        await Verify(await _context.Users.SingleAsync());
    }

    [Fact]
    public async Task AddUserTransientExceptionOccuredButWithoutMiddleware_OnSaveChangesAddTwoUsers()
    {
        var emptyContext = _fixture.CreateEmptyContext();
        
        await emptyContext.ExecuteExtendedAsync(async () =>
        {
            _context.Users.Add(new User(0, "Biba", false));

            _thrower.ThrowOnlyOnce();

            await _context.SaveChangesAsync();
        });

        _context.Clear();
        
        await Verify(await _context.Users.ToListAsync());
    }
}