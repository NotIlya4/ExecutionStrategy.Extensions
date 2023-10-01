namespace EntityFrameworkCore.ExecutionStrategyExtended.Configuration;

internal record ExecutionStrategyExtendedConfiguration : IExecutionStrategyExtendedConfiguration
{
    public Dictionary<object, object> Data { get; } = new();
}