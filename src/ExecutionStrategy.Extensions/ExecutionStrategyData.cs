namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public class ExecutionStrategyData : Dictionary<object, object>, IExecutionStrategyData
{
    
}

public interface IExecutionStrategyData : IDictionary<object, object>
{
    
}
