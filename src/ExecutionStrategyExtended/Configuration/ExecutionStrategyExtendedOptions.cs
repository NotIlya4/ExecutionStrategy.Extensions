namespace EntityFrameworkCore.ExecutionStrategyExtended.Configuration;

internal record ExecutionStrategyExtendedOptions : IExecutionStrategyExtendedOptionsBuilder
{
    public Dictionary<object, object> Data { get; } = new();
}