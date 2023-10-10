namespace EntityFrameworkCore.ExecutionStrategy.Extensions.Container;

internal class DependenciesContainer
{
    public MiddlewaresManager MiddlewaresManager { get; }
    public IExecutionStrategyData Data { get; }

    public DependenciesContainer(MiddlewaresManager middlewaresManager, IExecutionStrategyData data)
    {
        MiddlewaresManager = middlewaresManager;
        Data = data;
    }
}