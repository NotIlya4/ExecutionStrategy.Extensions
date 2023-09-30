using EntityFrameworkCore.ExecutionStrategyExtended.Builders;
using Microsoft.Extensions.Internal;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;

public interface IExecutionStrategyPublicConfiguration
{
    IBuilderPropertySetter<ISystemClock, IExecutionStrategyPublicConfiguration> SystemClock { get; }
    IBuilderPropertySetterConfig<DbContextRetrierConfiguration, IExecutionStrategyPublicConfiguration> DbContextRetrierConfiguration { get; }
    IBuilderPropertySetter<IIdempotenceViolationDetector, IExecutionStrategyPublicConfiguration> IdempotenceViolationDetector { get; }
    IBuilderPropertySetter<IResponseSerializer, IExecutionStrategyPublicConfiguration> ResponseSerializer { get; }
}