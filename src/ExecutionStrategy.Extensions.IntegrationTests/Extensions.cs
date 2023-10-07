using ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests;

public static class Extensions
{
    public static async Task ClearTables(this AppDbContext context)
    {
        context.RemoveRange(await context.Users.ToListAsync());
        await context.SaveChangesAsync();
    }

    public static void Clear(this AppDbContext context)
    {
        context.ChangeTracker.Clear();
    }

    public static async Task EnsureDeletedCreated(this AppDbContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
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