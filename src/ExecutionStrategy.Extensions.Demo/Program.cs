using EntityFrameworkCore.ExecutionStrategy.Extensions;
using ExecutionStrategy.Extensions.Demo;
using ExecutionStrategy.Extensions.Demo.EntityFramework;
using ExecutionStrategy.Extensions.Demo.Models;
using ExecutionStrategy.Extensions.Demo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddAppDbContext(builder.Configuration.GetConnectionString("Postgres")!);
services.AddScoped<UserService>();

var app = builder.Build();

app.MapGet("/users", async ([FromServices] UserService service, [FromServices] AppDbContext context) =>
{
    await context.ExecuteExtendedAsync(async () => { await service.GetUsers(); });
});

app.MapPost("/users",
    async (User user, [FromServices] UserService service, [FromServices] AppDbContext context) =>
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            context.ChangeTracker.Clear();
            await service.AddUser(user);
        });
    });

app.MapDelete("/users",
    async (int userId, [FromServices] UserService service, [FromServices] AppDbContext context) =>
    {
        await context.ExecuteExtendedAsync(async () => { await service.DeleteUser(userId); });
    });

app.Run();