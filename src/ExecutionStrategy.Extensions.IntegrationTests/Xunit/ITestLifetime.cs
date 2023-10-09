namespace ExecutionStrategy.Extensions.IntegrationTests;

public interface ITestLifetime
{
    public Task OnTestStart();
    public Task OnTestFinish();
}