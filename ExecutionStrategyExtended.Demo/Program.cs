using EntityFrameworkCore.ExecutionStrategyExtended;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddExecutionStrategyExtended<DbContext>(configuration =>
{
    
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();