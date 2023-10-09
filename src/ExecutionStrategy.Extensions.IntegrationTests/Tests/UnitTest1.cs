using EntityFrameworkCore.ExecutionStrategy.Extensions;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests.Tests;

public class UnitTest1
{
    private readonly TestFixture _fixture;
    private readonly TransientExceptionThrower _thrower;

    public UnitTest1(TestFixture fixture)
    {
        _fixture = fixture;
        _thrower = fixture.GetRequiredService<TransientExceptionThrower>();
    }
    
    [Fact]
    public async Task AddUserAndTransientExceptionOccured_OnSaveChangesSaveOnlyOneUser()
    {
        var context = _fixture.CreateWithClearChangeTracker();
        
        await context.ExecuteExtendedAsync(async () =>
        {
            context.Users.Add(new User(0, "Biba", false));

            _thrower.ThrowOnlyOnce();

            await context.SaveChangesAsync();
        });

        context.ChangeTrackerClear();

        await Verify(await context.Users.SingleAsync());
    }

    [Fact]
    public async Task AddUserTransientExceptionOccuredUseRegularExecutionStrategy_OnSaveChangesAddTwoUsers()
    {
        var context = _fixture.CreateContext();

        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            context.Users.Add(new User(0, "Biba", false));

            _thrower.ThrowOnlyOnce();

            await context.SaveChangesAsync();
        });

        context.ChangeTrackerClear();
        
        await Verify(await context.Users.ToListAsync());
    }
}