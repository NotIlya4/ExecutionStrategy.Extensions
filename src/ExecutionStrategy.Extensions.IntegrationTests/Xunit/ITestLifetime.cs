namespace ExecutionStrategy.Extensions.IntegrationTests.Xunit;

public interface ITestLifetime
{
    public Task OnTestStart();
    public Task OnTestFinish();
}