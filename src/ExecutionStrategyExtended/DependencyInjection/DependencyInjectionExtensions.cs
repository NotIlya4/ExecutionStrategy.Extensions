using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using EntityFrameworkCore.ExecutionStrategyExtended.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((_, _) => { });

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<ExecutionStrategyExtendedOptionsBuilder<TDbContext>> action) where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((_, options) => action(options));

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<IServiceProvider, ExecutionStrategyExtendedOptionsBuilder<TDbContext>> action)
        where TDbContext : DbContext
    {
        var lifetimeOverride = ServiceLifetime.Scoped;

        services.AddOptions();

        services.Add(new ServiceDescriptor(typeof(ActualDbContextProvider<TDbContext>),
            typeof(ActualDbContextProvider<TDbContext>), lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IActualDbContextProvider<TDbContext>),
            provider => provider.GetRequiredService<ActualDbContextProvider<TDbContext>>(), lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(ExecutionStrategyExtendedOptions<TDbContext>), Factory<TDbContext>,
            lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IConfigureOptions<ExecutionStrategyExtendedOptions<TDbContext>>),
            provider => new ConfigureOptions<TDbContext>(provider, action), lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(ExecutionStrategyExtended<TDbContext>),
            typeof(ExecutionStrategyExtended<TDbContext>), lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyExtended<TDbContext>),
            provider => provider.GetRequiredService<ExecutionStrategyExtended<TDbContext>>(), lifetimeOverride));

        return services;
    }

    private static ExecutionStrategyExtendedOptions<TDbContext> Factory<TDbContext>(IServiceProvider provider)
        where TDbContext : DbContext
    {
        var factory = provider.GetRequiredService<IOptionsFactory<ExecutionStrategyExtendedOptions<TDbContext>>>();
        return factory.Create(Microsoft.Extensions.Options.Options.DefaultName);
    }
}