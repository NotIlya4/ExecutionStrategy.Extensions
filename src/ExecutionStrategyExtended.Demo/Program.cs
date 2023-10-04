using EntityFrameworkCore.ExecutionStrategyExtended;
using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using EntityFrameworkCore.ExecutionStrategyExtended.DependencyInjection;
using ExecutionStrategyExtended.Demo.EntityFramework;
using ExecutionStrategyExtended.Demo.Models;
using ExecutionStrategyExtended.Demo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddDbContextFactory<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(config.GetConnectionString("Postgres")!,
        contextOptionsBuilder => contextOptionsBuilder.EnableRetryOnFailure());
});
services.AddExecutionStrategyExtended<AppDbContext>(configuration =>
{
    configuration
        .DbContextRetryBehavior.UseCreateNewDbContextRetryBehavior();
});
services.AddScoped<UserService>();

var app = builder.Build();

var context = app.Services.GetRequiredService<AppDbContext>();
var strategy = context.Database.CreateExecutionStrategy();

strategy.ExecuteExtendedAsync<AppDbContext, List<User>>(async dbContext => await dbContext.Users.ToListAsync());

app.MapGet("/", () => "Hello World!");

app.MapPost("/users",
    async (User user, [FromServices] UserService service,
        [FromServices] IExecutionStrategyExtended<AppDbContext> strategy) =>
    {
        await strategy.ExecuteAsync(async (context) => { await service.AddUser(user); });
    });

app.MapDelete("/users",
    async (int userId, [FromServices] UserService service,
        [FromServices] IExecutionStrategyExtended<AppDbContext> strategy) =>
    {
        await strategy.ExecuteAsync(async (context) => { await service.DeleteUser(userId); });
    });

app.Run();