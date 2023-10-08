using EntityFrameworkCore.ExecutionStrategy.Extensions;
using EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;
using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests;

public static class Extensions
{
    public static void ClearTables(this AppDbContext context)
    {
        context.RemoveRange(context.Users.ToList());
        context.SaveChanges();
    }

    public static void Clear(this AppDbContext context)
    {
        context.ChangeTracker.Clear();
    }

    public static void EnsureDeletedCreated(this DbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public static AppDbContext AppDbContext(this IServiceProvider provider)
    {
        return provider.GetRequiredService<AppDbContext>();
    }

    public static IServiceCollection ApplyDbInfrastructure(this IServiceCollection services, IDbInfrastructure db)
    {
        services.AddDbContext<AppDbContext>((provider, builder) =>
        {
            var db = provider.GetRequiredService<IIsolatedDbInfrastructure>();

            db.ConfigureDbContext(builder);
            builder.UseExecutionStrategyExtensions(builder => builder.WithClearChangeTrackerOnRetry());
        });
        services.AddSingleton(db);
        services.AddScoped<IIsolatedDbInfrastructure>(provider => provider.GetRequiredService<IDbInfrastructure>().ProvideIsolatedInfrastructure());
        return services;
    }
}