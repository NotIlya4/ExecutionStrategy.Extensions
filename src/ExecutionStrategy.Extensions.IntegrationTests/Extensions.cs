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

    public static void EnsureDeletedCreated(this AppDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public static AppDbContext AppDbContext(this IServiceProvider provider)
    {
        return provider.GetRequiredService<AppDbContext>();
    }

    public static IServiceCollection ApplyDbInfrastructure(this IServiceCollection services, IDbInfrastructureBuilder db)
    {
        db.ConfigureDbContext(services);
        services.AddSingleton(provider => db.CreateBootstrapper(provider.AppDbContext()));
        return services;
    }
}