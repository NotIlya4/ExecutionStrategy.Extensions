using EntityFrameworkCore.ExecutionStrategy.Extensions;
using EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ExecutionStrategy.Extensions.IntegrationTests.DbInfrastructure;

public interface IDbInfrastructureBuilder
{
    IDbInfrastructureBuilder UsePostgresExistingDb(PostgresExistingDbOptions options);
    IDbInfrastructureBuilder UsePostgresLocalContainer(PostgresLocalContainerOptions containerOptions);
    IDbInfrastructureBuilder UseSqlite();
    void ConfigureDbContext(IServiceCollection services);
    IDbBootstrapper CreateBootstrapper(AppDbContext context);
}

public class DbInfrastructureBuilder : IDbInfrastructureBuilder
{
    private Func<AppDbContext, IDbBootstrapper>? BootstrapperFactory { get; set; }
    private Action<DbContextOptionsBuilder>? DbContextOptionsConfigurator { get; set; }
    
    public IDbInfrastructureBuilder UsePostgresExistingDb(PostgresExistingDbOptions options)
    {
        BootstrapperFactory = context => new DelegateBootstrapper(
            () => options.OnBootstrap(context),
            () => options.OnClean(context), 
            () => options.OnDestroy(context));

        DbContextOptionsConfigurator = builder =>
        {
            var conn = new NpgsqlConnectionStringBuilder();
            conn["Host"] = "localhost";
            conn["Port"] = options.Port;
            conn["Username"] = options.Username;
            conn["Password"] = options.Password;
            conn["Database"] = options.Database;
            conn["Application Name"] = "ExecutionStrategy.Extensions";

            builder.UseNpgsql(conn.ConnectionString, optionsBuilder => optionsBuilder.EnableRetryOnFailure());
        };
        
        return this;
    }

    public IDbInfrastructureBuilder UsePostgresLocalContainer(PostgresLocalContainerOptions containerOptions)
    {
        var container = new PostgresContainer(new PostgresContainerOptions()
        {
            ContainerName = containerOptions.ContainerName,
            Image = containerOptions.Image,
            Password = containerOptions.Password,
            Port = containerOptions.Port
        });
        BootstrapperFactory = context => new DelegateBootstrapper(
            () => containerOptions.OnBootstrap(container, context),
            () => containerOptions.OnClean(container, context), 
            () => containerOptions.OnDestroy(container, context));

        DbContextOptionsConfigurator = builder =>
        {
            var conn = new NpgsqlConnectionStringBuilder();
            conn["Host"] = "localhost";
            conn["Port"] = containerOptions.Port;
            conn["Username"] = containerOptions.Username;
            conn["Password"] = containerOptions.Password;
            conn["Database"] = containerOptions.Database;
            conn["Application Name"] = "ExecutionStrategy.Extensions";

            builder.UseNpgsql(conn.ConnectionString, optionsBuilder => optionsBuilder.EnableRetryOnFailure());
        };

        return this;
    }

    public IDbInfrastructureBuilder UseSqlite()
    {
        BootstrapperFactory = context => new DelegateBootstrapper(
            context.EnsureDeletedCreated,
            context.EnsureDeletedCreated, 
            context.EnsureDeletedCreated);

        DbContextOptionsConfigurator = builder =>
        {
            builder.UseSqlite("Filename=:memory:");
        };
        
        return this;
    }

    public void ConfigureDbContext(IServiceCollection services)
    {
        services.AddDbContextFactory<AppDbContext>(builder =>
        {
            DbContextOptionsConfigurator!(builder);
            builder.UseExecutionStrategyExtensions(optionsBuilder => optionsBuilder.WithClearChangeTrackerOnRetry());
        });
    }

    public IDbBootstrapper CreateBootstrapper(AppDbContext context)
    {
        return BootstrapperFactory!(context);
    }
}