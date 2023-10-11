using System.Data;
using EntityFrameworkCore.ExecutionStrategy.Extensions;
using EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using ExecutionStrategy.Extensions.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ExecutionStrategy.Extensions.IntegrationTests.Tests;

public class ExecuteExtendedAsyncTests
{
    private readonly TestFixture _fixture;

    public ExecuteExtendedAsyncTests(TestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ExecuteExtendedAsync_AddUserAndTransientExceptionOccured_OnSaveChangesSaveOnlyOneUser()
    {
        var thrower = Hooker.ThrowTransientOnce();
        var context = _fixture.CreateDbContextWithClearChangeTracker();

        await context.ExecuteExtendedAsync(async () =>
        {
            context.Users.Add(new User(0, "asd", false));

            thrower.Trigger();
            await Verify(context.ChangeTracker).UseTextForParameters("change-tracker");

            await context.SaveChangesAsync();
        });

        context.ChangeTrackerClear();
        Assert.Equal(1, await context.Users.CountAsync());
    }

    [Fact]
    public async Task ExecuteExtendedAsync_AddUserTransientExceptionOccuredUseRegularExecutionStrategy_OnSaveChangesAddTwoUsers()
    {
        var thrower = Hooker.ThrowTransientOnce();
        var context = _fixture.CreateDbContext();
        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            context.Users.Add(new User(0, "asd", false));

            thrower.Trigger();
            await Verify(context.ChangeTracker).UseTextForParameters("change-tracker");

            await context.SaveChangesAsync();
        });

        context.ChangeTrackerClear();
        Assert.Equal(2, await context.Users.CountAsync());
    }

    [Fact]
    public async Task ExecuteExtendedAsync_EmptyMiddlewares_OperationInvoked()
    {
        var checker = Substitute.For<IChecker>();
        var context = _fixture.CreateDbContextWithEmptyUse();

        await context.ExecuteExtendedAsync(() =>
        {
            checker.Check();
            return Task.CompletedTask;
        });

        Received.InOrder(() => { checker.Check(); });
    }

    [Fact]
    public async Task ExecuteExtendedAsync_ProvideMiddlewares_MiddlewaresCallInRightOrder()
    {
        var checker = Substitute.For<IChecker>();
        var context = _fixture.CreateDbContextWithEmptyUse();

        await context.ExecuteExtendedAsync(() =>
            {
                checker.Check("3");
                return Task.CompletedTask;
            },
            builder =>
            {
                builder
                    .WithMiddleware(async (next, args) =>
                    {
                        checker.Check("2");
                        await next(args);
                        checker.Check("4");
                    })
                    .WithMiddleware(async (next, args) =>
                    {
                        checker.Check("1");
                        await next(args);
                        checker.Check("5");
                    });
            });

        Received.InOrder(() =>
        {
            checker.Check("1");
            checker.Check("2");
            checker.Check("3");
            checker.Check("4");
            checker.Check("5");
        });
    }

    [Fact]
    public async Task ExecuteExtendedAsync_OmitNextInsideMiddleware_OperationNotCalled()
    {
        var checker = Substitute.For<IChecker>();
        var context = _fixture.CreateDbContextWithEmptyUse();

        await context.ExecuteExtendedAsync(() =>
            {
                checker.Check();
                return Task.CompletedTask;
            },
            builder => builder.WithMiddleware((_, _) =>
            {
                checker.Check("1");
                checker.Check("2");
                return Task.CompletedTask;
            }));

        Received.InOrder(() =>
        {
            checker.Check("1");
            checker.Check("2");
        });
    }

    [Fact]
    public async Task ExecuteExtendedAsync_ProvideVerifySucceededAndTransientExceptionThrown_CallVerifySucceeded()
    {
        var checker = Substitute.For<IChecker>();
        var thrower = Hooker.ThrowTransientOnce();
        var context = _fixture.CreateDbContextWithEmptyUse();

        await context.ExecuteExtendedAsync(() =>
            {
                thrower.Trigger();
                return Task.CompletedTask;
            },
            builder => builder.WithVerifySucceeded(_ =>
            {
                checker.Check();
                return Task.FromResult(true);
            }));

        Received.InOrder(() =>
        {
            checker.Check();
        });
    }

    [Fact]
    public async Task ExecuteExtendedAsync_ProvideDefaultMiddleware_RunMiddlewareEachTime()
    {
        var checker = Substitute.For<IChecker>();
        var context = _fixture.CreateDbContext(builder =>
        {
            builder.UseExecutionStrategyExtensions(builder =>
            {
                builder.WithMiddleware(async (next, args) =>
                {
                    checker.Check("1");
                    var result = await next(args);
                    checker.Check("3");
                    return result;
                });
            });
        });

        await context.ExecuteExtendedAsync(() =>
        {
            checker.Check("2");
            return Task.CompletedTask;
        });
        
        Received.InOrder(() =>
        {
            checker.Check("1");
            checker.Check("2");
            checker.Check("3");
        });
    }
    
    [Fact]
    public async Task ExecuteExtendedAsync_ProvideDefaultData_WhenAccessDataCanGetDefaultData()
    {
        var context = _fixture.CreateDbContext(builder =>
        {
            builder.UseExecutionStrategyExtensions(builder =>
            {
                builder.WithData(data => data["asd"] = "asd");
            });
        });

        await context.ExecuteExtendedAsync((args) =>
        {
            Assert.Equal("asd", args.Data["asd"]);
            return Task.CompletedTask;
        });
    }

    [Fact]
    public async Task ExecuteExtendedAsync_UseWithTransactionThrowException_DontSaveChanges()
    {
        var context = _fixture.CreateDbContextWithClearChangeTracker();

        try
        {
            await context.ExecuteExtendedAsync(async () =>
            {
                context.Users.Add(new User(0, "asd", false));
                await context.SaveChangesAsync();
                throw new Exception();
            }, builder =>
            {
                builder.WithTransaction(IsolationLevel.Serializable);
            });
        }
        catch (Exception) { }
        
        context.ChangeTracker.Clear();
        Assert.Equal(0, await context.Users.CountAsync());
    }
    
    [Fact]
    public async Task ExecuteExtendedAsync_UseWithTransaction_ChangesSaved()
    {
        var context = _fixture.CreateDbContextWithClearChangeTracker();

        await context.ExecuteExtendedAsync(async () =>
        {
            context.Users.Add(new User(0, "asd", false));
            await context.SaveChangesAsync();
        }, builder =>
        {
            builder.WithTransaction(IsolationLevel.Serializable);
        });
        
        context.ChangeTracker.Clear();
        Assert.Equal(1, await context.Users.CountAsync());
    }
}