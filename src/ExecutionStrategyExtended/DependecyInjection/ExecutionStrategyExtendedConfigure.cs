using EntityFrameworkCore.ExecutionStrategyExtended.Configuration;
using Microsoft.Extensions.Options;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;

internal class ExecutionStrategyExtendedConfigure : IConfigureOptions<ExecutionStrategyExtendedConfiguration>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Action<IServiceProvider, IExecutionStrategyExtendedConfiguration> _action;

    public ExecutionStrategyExtendedConfigure(IServiceProvider serviceProvider, Action<IServiceProvider, IExecutionStrategyExtendedConfiguration> action)
    {
        _serviceProvider = serviceProvider;
        _action = action;
    }
    
    public void Configure(ExecutionStrategyExtendedConfiguration extendedConfiguration)
    {
        _action(_serviceProvider, extendedConfiguration);
    }
}