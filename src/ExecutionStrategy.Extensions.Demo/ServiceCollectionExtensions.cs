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
            builder.UseNpgsql(conn, contextOptionsBuilder => contextOptionsBuilder.EnableRetryOnFailure());
            
            builder.UseExecutionStrategyExtensions<AppDbContext>(
                primaryOptionsBuilder => primaryOptionsBuilder.WithClearChangeTrackerOnRetry());
        });
        
        return services;
    }
}