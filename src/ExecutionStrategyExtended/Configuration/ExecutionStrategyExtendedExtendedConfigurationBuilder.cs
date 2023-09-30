using EntityFrameworkCore.ExecutionStrategyExtended.Builders;
using EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;
using Microsoft.Extensions.Internal;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

internal record ExecutionStrategyExtendedExtendedConfigurationBuilder : IExecutionStrategyConfigurationBuilder, IExecutionStrategyExtendedConfiguration
{
    private IBuilderPropertySetter<ISystemClock, IExecutionStrategyConfigurationBuilder> _systemClockBuilder;
    private IBuilderPropertySetterConfig<DbContextRetrierConfiguration, IExecutionStrategyConfigurationBuilder> _dbContextRetrierConfigurationBuilder;
    private IBuilderPropertySetter<IIdempotenceViolationDetector, IExecutionStrategyConfigurationBuilder> _idempotenceViolationDetectorBuilder;
    private IBuilderPropertySetter<IResponseSerializer, IExecutionStrategyConfigurationBuilder> _responseSerializerBuilder;
    private ISystemClock _systemClock = new SystemClock();
    private DbContextRetrierConfiguration _dbContextRetrierConfiguration = DbContextRetriers.NewDbContextRetrier();
    private IIdempotenceViolationDetector? _idempotenceViolationDetector;
    private IResponseSerializer? _responseSerializer;

    public ExecutionStrategyExtendedExtendedConfigurationBuilder()
    {
        _systemClockBuilder =
            new BuilderProperty<ISystemClock, IExecutionStrategyConfigurationBuilder>(clock => _systemClock = clock, this,
                _systemClock);
        
        _dbContextRetrierConfigurationBuilder =
            new BuilderProperty<DbContextRetrierConfiguration, IExecutionStrategyConfigurationBuilder>(
                configuration => _dbContextRetrierConfiguration = configuration, this,
                _dbContextRetrierConfiguration);
        
        _idempotenceViolationDetectorBuilder =
            new BuilderProperty<IIdempotenceViolationDetector, IExecutionStrategyConfigurationBuilder>(
                detector => _idempotenceViolationDetector = detector, this);

        _responseSerializerBuilder = new BuilderProperty<IResponseSerializer, IExecutionStrategyConfigurationBuilder>(
            serializer => _responseSerializer = serializer, this);
    }


    ISystemClock IExecutionStrategyExtendedConfiguration.SystemClock => _systemClock;
    DbContextRetrierConfiguration IExecutionStrategyExtendedConfiguration.DbContextRetrierConfiguration => _dbContextRetrierConfiguration;
    IIdempotenceViolationDetector? IExecutionStrategyExtendedConfiguration.IdempotenceViolationDetector => _idempotenceViolationDetector;
    IResponseSerializer IExecutionStrategyExtendedConfiguration.ResponseSerializer => _responseSerializer!;

    IBuilderPropertySetter<ISystemClock, IExecutionStrategyConfigurationBuilder> IExecutionStrategyConfigurationBuilder.SystemClock => _systemClockBuilder;
    IBuilderPropertySetter<DbContextRetrierConfiguration, IExecutionStrategyConfigurationBuilder> IExecutionStrategyConfigurationBuilder.DbContextRetrierConfiguration => _dbContextRetrierConfigurationBuilder;
    IBuilderPropertySetter<IIdempotenceViolationDetector, IExecutionStrategyConfigurationBuilder> IExecutionStrategyConfigurationBuilder.IdempotenceViolationDetector => _idempotenceViolationDetectorBuilder;
    IBuilderPropertySetter<IResponseSerializer, IExecutionStrategyConfigurationBuilder> IExecutionStrategyConfigurationBuilder.ResponseSerializer => _responseSerializerBuilder;
}