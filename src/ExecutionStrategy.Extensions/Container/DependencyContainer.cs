namespace EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;

internal class DependencyContainer
{
    public MiddlewaresManager MiddlewaresManager { get; }
    public IExecutionStrategyData Data { get; }

    public DependencyContainer(MiddlewaresManager middlewaresManager, IExecutionStrategyData data)
    {
        MiddlewaresManager = middlewaresManager;
        Data = data;
    }
}