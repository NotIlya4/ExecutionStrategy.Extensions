using EntityFrameworkCore.ExecutionStrategy.Extensions;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VerifyTests.EntityFramework;

namespace ExecutionStrategy.Extensions.IntegrationTests.Tests;

public interface IRunnable
{
    void Tick();
}

public class ExtensionHighLevelTests
{
    private readonly TestFixture _fixture;

    public ExtensionHighLevelTests(TestFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task AddUserAndTransientExceptionOccured_OnSaveChangesSaveOnlyOneUser()
    {
        var mock = new Mock<IRunnable>();
        mock.SetupSequence()
            
        var obj = mock.Object;

        var thrower = Thrower.ThrowOnFirstCall();
        var context = _fixture.CreateWithClearChangeTracker();
        
        EfRecording.StartRecording();
        await context.ExecuteExtendedAsync(async () =>
        {
            context.Users.Add(new User(0, "Biba", false));
            
            thrower.Tick();
            await Verify(context.ChangeTracker).UseTextForParameters("change-tracker");

            await context.SaveChangesAsync();
        });
        await Verify().UseTextForParameters("sql");
        EfRecording.FinishRecording();

        context.ChangeTrackerClear();
        await Verify(await context.Users.SingleAsync());
    }

    [Fact]
    public async Task AddUserTransientExceptionOccuredUseRegularExecutionStrategy_OnSaveChangesAddTwoUsers()
    {
        var context = _fixture.CreateContext();
        var thrower = Thrower.ThrowOnFirstCall();
        var strategy = context.Database.CreateExecutionStrategy();

        EfRecording.StartRecording();
        await strategy.ExecuteAsync(async () =>
        {
            context.Users.Add(new User(0, "Biba", false));
            
            thrower.Tick();
            await Verify(context.ChangeTracker).UseTextForParameters("change-tracker");

            await context.SaveChangesAsync();
        });
        await Verify().UseTextForParameters("sql");
        EfRecording.FinishRecording();

        context.ChangeTrackerClear();
        await Verify(await context.Users.ToListAsync());
    }
}