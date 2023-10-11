using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.Exceptions;

/// <summary>
/// Exception that thrown when you try to use ExecuteExtendedAsync without registering services inside <see cref="DbContextOptionsBuilder{TContext}"/>
/// </summary>
public class ServicesForExecutionStrategyExtensionsNotFound : InvalidOperationException
{
    /// <inheritdoc />
    public ServicesForExecutionStrategyExtensionsNotFound()
    {
    }

    /// <inheritdoc />
    protected ServicesForExecutionStrategyExtensionsNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <inheritdoc />
    public ServicesForExecutionStrategyExtensionsNotFound(string? message) : base(message)
    {
    }

    /// <inheritdoc />
    public ServicesForExecutionStrategyExtensionsNotFound(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}