using EntityFrameworkCore.ExecutionStrategyExtended.Builders;
using Microsoft.Extensions.Internal;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;

public interface IExecutionStrategyConfigurationBuilder
{
    IBuilderPropertySetter<ISystemClock, IExecutionStrategyConfigurationBuilder> SystemClock { get; }
    IBuilderPropertySetter<DbContextRetrierConfiguration, IExecutionStrategyConfigurationBuilder> DbContextRetrierConfiguration { get; }
    IBuilderPropertySetter<IIdempotenceViolationDetector, IExecutionStrategyConfigurationBuilder> IdempotenceViolationDetector { get; }
    IBuilderPropertySetter<IResponseSerializer, IExecutionStrategyConfigurationBuilder> ResponseSerializer { get; }
}