namespace EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;

internal class ExecutionStrategyContainer
{
    public ExecutionStrategyMiddlewaresManager MiddlewaresManager { get; }
    public IExecutionStrategyData Data { get; }

    public ExecutionStrategyContainer(ExecutionStrategyMiddlewaresManager middlewaresManager, IExecutionStrategyData data)
    {
        MiddlewaresManager = middlewaresManager;
        Data = data;
    }
}