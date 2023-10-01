using EntityFrameworkCore.ExecutionStrategyExtended.Configuration;
using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((_, _) => { });

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<IExecutionStrategyExtendedConfiguration> action) where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((_, options) => action(options));

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<IServiceProvider, IExecutionStrategyExtendedConfiguration> action) where TDbContext : DbContext
    {
        var lifetimeOverride = ServiceLifetime.Scoped;

        services.AddOptions();

        services.AddTransient<IConfigureOptions<ExecutionStrategyExtendedConfiguration>, ExecutionStrategyExtendedConfigure>(
            provider => new ExecutionStrategyExtendedConfigure(provider, action));

        services.Add(new ServiceDescriptor(typeof(ActualDbContextProvider<TDbContext>),
            typeof(ActualDbContextProvider<TDbContext>), lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IActualDbContextProvider<TDbContext>),
            provider => provider.GetRequiredService<ActualDbContextProvider<TDbContext>>(), lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(ExecutionStrategyExtendedConfiguration), Factory, lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyExtendedConfiguration),
            provider => provider.GetRequiredService<ExecutionStrategyExtendedConfiguration>(), lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(ExecutionStrategyExtended<TDbContext>),
            provider =>
            {
                var configuration = provider.GetRequiredService<IExecutionStrategyExtendedConfiguration>();
                var dbContextFactory = provider.GetRequiredService<IDbContextFactory<TDbContext>>();
                var mainContext = provider.GetRequiredService<TDbContext>();
                var actualDbContextProvider = provider.GetRequiredService<ActualDbContextProvider<TDbContext>>();

                var retrierFactory = new DbContextRetrierFactory<TDbContext>(configuration.RetrierConfiguration(), dbContextFactory);
                
                return new ExecutionStrategyExtended<TDbContext>(retrierFactory, mainContext, configuration, actualDbContextProvider);
            }, lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyExtended<TDbContext>),
            provider => provider.GetRequiredService<ExecutionStrategyExtended<TDbContext>>(), lifetimeOverride));

        return services;
    }

    private static ExecutionStrategyExtendedConfiguration Factory(IServiceProvider provider)
    {
        var factory = provider.GetRequiredService<IOptionsFactory<ExecutionStrategyExtendedConfiguration>>();
        return factory.Create(Options.DefaultName);
    }

    public static DbContextRetrierConfigurationPart DbContextRetrierConfiguration(
        this IExecutionStrategyExtendedConfiguration configuration)
    {
        return new DbContextRetrierConfigurationPart(configuration);
    }
}