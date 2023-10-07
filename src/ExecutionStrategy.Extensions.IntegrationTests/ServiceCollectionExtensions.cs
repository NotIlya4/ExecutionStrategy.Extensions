using EntityFrameworkCore.ExecutionStrategy.Extensions;
using EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;
using ExecutionStrategy.Extensions.IntegrationTests.DbContextConfigurator;
using ExecutionStrategy.Extensions.IntegrationTests.PostgresBootstrapping;
using ExecutionStrategy.Extensions.IntegrationTests.PostgresBootstrapping.ExistingDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IDbContextOptions options)
    {
        services.AddDbContext<AppDbContext>(builder =>
        {
            switch (options)
            {
                case PostgresDbContextOptions postgres:
                    builder.UseNpgsql(postgres.Conn,
                        contextOptionsBuilder => contextOptionsBuilder.EnableRetryOnFailure());
                    break;

                case SqliteDbContextOptions:
                    builder.UseSqlite("Filename=:memory:");
                    break;
            }

            builder.UseExecutionStrategyExtensions<AppDbContext>(
                optionsBuilder => optionsBuilder.WithClearChangeTrackerOnRetry());
        });

        return services;
    }

    public static IServiceCollection AddBootstrapper(this IServiceCollection services)
    {
        services.AddScoped<IDbBootstrapper, ExistingDbBootstrapper>(provider => new ExistingDbBootstrapper(
            provider.GetRequiredService<AppDbContext>(), new ExistingDbOptions()
            {
                OnBootstrap = ExistingDbOptions.OnBootstrapType.CleanTables,
                OnClean = ExistingDbOptions.OnCleanType.CleanTables,
                OnDestroy = ExistingDbOptions.OnDestroyType.CleanTables
            }));
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

public interface IDbContextOptions
{
}