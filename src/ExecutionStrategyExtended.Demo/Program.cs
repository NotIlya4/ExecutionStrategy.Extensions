using EntityFrameworkCore.ExecutionStrategy.Extensions;
using ExecutionStrategyExtended.Demo;
using ExecutionStrategyExtended.Demo.EntityFramework;
using ExecutionStrategyExtended.Demo.Models;
using ExecutionStrategyExtended.Demo.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddAppDbContext(new SqliteDbContextOptions());
services.AddScoped<UserService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/users",
    async (User user, [FromServices] UserService service, [FromServices] AppDbContext context) =>
    {
        await context.ExecuteExtendedAsync(async () => { await service.AddUser(user); return true; });
    });

app.MapDelete("/users",
    async (int userId, [FromServices] UserService service, [FromServices] AppDbContext context) =>
    {
        await context.ExecuteExtendedAsync(async () => { await service.DeleteUser(userId); return true; });
    });

app.Run();