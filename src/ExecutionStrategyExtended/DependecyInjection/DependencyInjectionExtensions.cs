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
        Action<IExecutionStrategyExtendedOptionsBuilder> action) where TDbContext : DbContext
    {
        services.AddExecutionStrategyExtended<TDbContext>((_, options) => action(options));

        return services;
    }

    public static IServiceCollection AddExecutionStrategyExtended<TDbContext>(this IServiceCollection services,
        Action<IServiceProvider, IExecutionStrategyExtendedOptionsBuilder> action) where TDbContext : DbContext
    {
        var lifetimeOverride = ServiceLifetime.Scoped;

        services.AddOptions();

        services.AddTransient<IConfigureOptions<ExecutionStrategyExtendedOptions>, ExecutionStrategyExtendedConfigure>(
            provider => new ExecutionStrategyExtendedConfigure(provider, action));

        services.Add(new ServiceDescriptor(typeof(ActualDbContextProvider<TDbContext>),
            typeof(ActualDbContextProvider<TDbContext>), lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IActualDbContextProvider<TDbContext>),
            provider => provider.GetRequiredService<ActualDbContextProvider<TDbContext>>(), lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(ExecutionStrategyExtendedOptions), Factory, lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyExtendedOptionsBuilder),
            provider => provider.GetRequiredService<ExecutionStrategyExtendedOptions>(), lifetimeOverride));

        services.Add(new ServiceDescriptor(typeof(ExecutionStrategyExtended<TDbContext>),
            provider =>
            {
                var options = provider.GetRequiredService<ExecutionStrategyExtendedOptions>();
                var dbContextFactory = provider.GetRequiredService<IDbContextFactory<TDbContext>>();
                var mainContext = provider.GetRequiredService<TDbContext>();
                var actualDbContextProvider = provider.GetRequiredService<ActualDbContextProvider<TDbContext>>();

                var retrierFactory = new DbContextRetrierFactory<TDbContext>(options.RetryBehaviorOptions(), dbContextFactory);
                
                return new ExecutionStrategyExtended<TDbContext>(retrierFactory, mainContext, options, actualDbContextProvider);
            }, lifetimeOverride));
        services.Add(new ServiceDescriptor(typeof(IExecutionStrategyExtended<TDbContext>),
            provider => provider.GetRequiredService<ExecutionStrategyExtended<TDbContext>>(), lifetimeOverride));

        return services;
    }

    private static ExecutionStrategyExtendedOptions Factory(IServiceProvider provider)
    {
        var factory = provider.GetRequiredService<IOptionsFactory<ExecutionStrategyExtendedOptions>>();
        return factory.Create(Options.DefaultName);
    }

    public static DbContextRetrierOptionsPart DbContextRetryBehaviorOptions(
        this IExecutionStrategyExtendedOptionsBuilder optionsBuilder)
    {
        return new DbContextRetrierOptionsPart((ExecutionStrategyExtendedOptions)optionsBuilder);
    }
}