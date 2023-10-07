using EntityFrameworkCore.ExecutionStrategy.Extensions;
using ExecutionStrategy.Extensions.Demo;
using ExecutionStrategy.Extensions.Demo.EntityFramework;
using ExecutionStrategy.Extensions.Demo.Models;
using ExecutionStrategy.Extensions.Demo.Services;
using ExecutionStrategyExtended.Demo;
using ExecutionStrategyExtended.Demo.EntityFramework;
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
        await context.ExecuteExtendedAsync(async () => { await service.AddUser(user); });
    });

app.MapDelete("/users",
    async (int userId, [FromServices] UserService service, [FromServices] AppDbContext context) =>
    {
        await context.ExecuteExtendedAsync(async () => { await service.DeleteUser(userId); });
    });

app.Run();