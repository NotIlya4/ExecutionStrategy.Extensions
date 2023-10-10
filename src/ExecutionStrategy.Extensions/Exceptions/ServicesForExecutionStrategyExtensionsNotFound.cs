using System.Runtime.Serialization;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.Exceptions;

public class ServicesForExecutionStrategyExtensionsNotFound : InvalidOperationException
{
    public ServicesForExecutionStrategyExtensionsNotFound()
    {
    }

    protected ServicesForExecutionStrategyExtensionsNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ServicesForExecutionStrategyExtensionsNotFound(string? message) : base(message)
    {
    }

    public ServicesForExecutionStrategyExtensionsNotFound(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}