using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <inheritdoc cref="IExecutionStrategyData" />
public class ExecutionStrategyData : Dictionary<object, object>, IExecutionStrategyData
{
    
}

/// <summary>
/// It can be used for any custom data. You can pass it from <see cref="DbContextOptionsBuilder{TContext}"/> or inside action builder.
/// </summary>
public interface IExecutionStrategyData : IDictionary<object, object>
{
    
}
