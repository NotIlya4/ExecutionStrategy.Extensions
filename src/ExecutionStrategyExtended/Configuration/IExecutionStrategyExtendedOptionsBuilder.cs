namespace EntityFrameworkCore.ExecutionStrategyExtended.Configuration;

public interface IExecutionStrategyExtendedOptionsBuilder
{
    Dictionary<object, object> Data { get; }
}