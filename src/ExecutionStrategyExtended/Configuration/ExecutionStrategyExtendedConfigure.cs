using EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;
using Microsoft.Extensions.Options;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

internal class ExecutionStrategyExtendedConfigure : IConfigureOptions<ExecutionStrategyExtendedConfiguration>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Action<IServiceProvider, IExecutionStrategyPublicConfiguration> _action;

    public ExecutionStrategyExtendedConfigure(IServiceProvider serviceProvider, Action<IServiceProvider, IExecutionStrategyPublicConfiguration> action)
    {
        _serviceProvider = serviceProvider;
        _action = action;
    }
    
    public void Configure(ExecutionStrategyExtendedConfiguration configuration)
    {
        _action(_serviceProvider, configuration);
    }
}