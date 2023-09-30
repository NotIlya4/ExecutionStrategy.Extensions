using EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;
using Microsoft.Extensions.Options;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

internal class ExecutionStrategyExtendedConfigure : IConfigureOptions<ExecutionStrategyExtendedExtendedConfigurationBuilder>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Action<IServiceProvider, IExecutionStrategyConfigurationBuilder> _action;

    public ExecutionStrategyExtendedConfigure(IServiceProvider serviceProvider, Action<IServiceProvider, IExecutionStrategyConfigurationBuilder> action)
    {
        _serviceProvider = serviceProvider;
        _action = action;
    }
    
    public void Configure(ExecutionStrategyExtendedExtendedConfigurationBuilder extendedConfigurationBuilder)
    {
        _action(_serviceProvider, extendedConfigurationBuilder);
    }
}