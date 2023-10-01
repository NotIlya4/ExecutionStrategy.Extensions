using EntityFrameworkCore.ExecutionStrategyExtended.DependecyInjection;
using ExecutionStrategyExtended.Demo;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();