using EntityFrameworkCore.ExecutionStrategy.Extensions;
using EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;
using ExecutionStrategy.Extensions.Demo.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategy.Extensions.Demo;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IDbContextOptions options)
    {
        services.AddDbContext<AppDbContext>(builder =>
        {
            switch (options)
            {
                case PostgresDbContextOptions postgres:
                    builder.UseNpgsql(postgres.Conn, contextOptionsBuilder => contextOptionsBuilder.EnableRetryOnFailure());
                    break;
                
                case SqliteDbContextOptions:
                    builder.UseSqlite("Filename=:memory:");
                    break;
            }

            builder.UseExecutionStrategyExtensions<AppDbContext>(
                primaryOptionsBuilder => primaryOptionsBuilder.WithClearChangeTrackerOnRetry());
        });
        
        return services;
    }
}

public class PostgresDbContextOptions : IDbContextOptions
{
    public required string Conn { get; set; }
}

public class SqliteDbContextOptions : IDbContextOptions
{
    
}

public interface IDbContextOptions { }