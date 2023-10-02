using EntityFrameworkCore.ExecutionStrategyExtended.Configuration;
using Microsoft.Extensions.Options;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;

internal class ExecutionStrategyExtendedConfigure : IConfigureOptions<ExecutionStrategyExtendedOptions>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Action<IServiceProvider, IExecutionStrategyExtendedOptionsBuilder> _action;

    public ExecutionStrategyExtendedConfigure(IServiceProvider serviceProvider, Action<IServiceProvider, IExecutionStrategyExtendedOptionsBuilder> action)
    {
        _serviceProvider = serviceProvider;
        _action = action;
    }
    
    public void Configure(ExecutionStrategyExtendedOptions extendedOptions)
    {
        _action(_serviceProvider, extendedOptions);
    }
}