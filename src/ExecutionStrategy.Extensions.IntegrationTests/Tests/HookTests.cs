using ExecutionStrategy.Extensions.IntegrationTests.HookClass;
using NSubstitute;

namespace ExecutionStrategy.Extensions.IntegrationTests.Tests;

public class HookTests
{
    [Fact]
    public void Hook_SpamHook_CallFirst()
    {
        var checker = Substitute.For<IChecker>();
        var hooker = new Hooker(() => checker.Check(), 1);

        hooker.Trigger();
        hooker.Trigger();
        hooker.Trigger();
        
        Received.InOrder(() =>
        {
            checker.Check();
        });
    }
    
    [Fact]
    public void ThrowTransientOnce_Spam_ThrowOnce()
    {
        var hooker = Hooker.ThrowTransientOnce();

        Assert.ThrowsAny<Exception>(hooker.Trigger);
        hooker.Trigger();
        hooker.Trigger();
    }
}