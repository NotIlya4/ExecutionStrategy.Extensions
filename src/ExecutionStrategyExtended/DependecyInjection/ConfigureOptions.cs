using EntityFrameworkCore.ExecutionStrategyExtended.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;

internal class ConfigureOptions<TDbContext> : IConfigureOptions<ExecutionStrategyExtendedOptions<TDbContext>> where TDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Action<IServiceProvider, ExecutionStrategyExtendedOptionsBuilder<TDbContext>> _action;

    public ConfigureOptions(IServiceProvider serviceProvider, Action<IServiceProvider, ExecutionStrategyExtendedOptionsBuilder<TDbContext>> action)
    {
        _serviceProvider = serviceProvider;
        _action = action;
    }
    
    public void Configure(ExecutionStrategyExtendedOptions<TDbContext> options)
    {
        var builder = new ExecutionStrategyExtendedOptionsBuilder<TDbContext>(_serviceProvider, options);
        _action(_serviceProvider, builder);
    }
}