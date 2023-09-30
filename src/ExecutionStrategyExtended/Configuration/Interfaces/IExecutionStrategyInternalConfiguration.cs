using Microsoft.Extensions.Internal;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;

internal interface IExecutionStrategyInternalConfiguration
{
    ISystemClock SystemClock { get; }
    DbContextRetrierConfiguration DbContextRetrierConfiguration { get; }
    IIdempotenceViolationDetector? IdempotenceViolationDetector { get; }
    IResponseSerializer ResponseSerializer { get; }
}