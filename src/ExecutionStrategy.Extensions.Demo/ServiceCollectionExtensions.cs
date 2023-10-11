using EntityFrameworkCore.ExecutionStrategy.Extensions;
using EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;
using ExecutionStrategy.Extensions.Demo.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategy.Extensions.Demo;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, string conn)
    {
        services.AddDbContext<AppDbContext>(builder =>
        {
            builder.UseNpgsql(conn, builder => 
                builder.EnableRetryOnFailure());

            builder.UseExecutionStrategyExtensions<AppDbContext>(
                builder => builder.WithClearChangeTrackerOnRetry());
        });
        
        return services;
    }
}