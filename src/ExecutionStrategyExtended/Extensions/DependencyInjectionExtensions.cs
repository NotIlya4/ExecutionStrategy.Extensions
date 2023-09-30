using EntityFrameworkCore.ExecutionStrategyExtended.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((_, _) => { });

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<IExecutionStrategyConfigurationBuilder> action) where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((_, options) => action(options));

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<IServiceProvider, IExecutionStrategyConfigurationBuilder> action) where TDbContext : DbContext
    {
        var lifetimeOverride = ServiceLifetime.Scoped;
        
        services.AddOptions();

        services
            .AddTransient<IConfigureOptions<ExecutionStrategyExtendedExtendedConfigurationBuilder>,
                ExecutionStrategyExtendedConfigure>(
                provider => new ExecutionStrategyExtendedConfigure(provider, action));

        services.Add(new ServiceDescriptor(typeof(ActualDbContextProvider<TDbContext>), typeof(ActualDbContextProvider<TDbContext>), lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IActualDbContextProvider<TDbContext>), typeof(ActualDbContextProvider<TDbContext>), lifetimeOverride));
        
        services.Add(new ServiceDescriptor(typeof(ExecutionStrategyExtendedExtendedConfigurationBuilder), Factory, lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyExtendedConfiguration), Factory, lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyConfigurationBuilder), Factory, lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(MainFactory<TDbContext>), typeof(MainFactory<TDbContext>), lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyExtended<TDbContext>),
            typeof(ExecutionStrategyExtended<TDbContext>),
            lifetimeOverride));
        
        services.Add(new ServiceDescriptor(typeof(ExecutionStrategyExtended<TDbContext>), typeof(ExecutionStrategyExtended<TDbContext>), lifetimeOverride));

        return services;
    }

    private static ExecutionStrategyExtendedExtendedConfigurationBuilder Factory(IServiceProvider provider)
    {
        var factory = provider.GetRequiredService<IOptionsFactory<ExecutionStrategyExtendedExtendedConfigurationBuilder>>();
        return factory.Create(Options.DefaultName);
    }
}