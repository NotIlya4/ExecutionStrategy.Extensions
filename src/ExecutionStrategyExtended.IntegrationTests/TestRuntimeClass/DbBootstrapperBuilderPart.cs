using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategyExtended.UnitTests;

public class DbBootstrapperBuilderPart
{
    public FluentDockerOptions? FluentDockerOptions { get; set; }
    public ExistingDbOptions? ExistingDbOptions { get; set; }
    public DbBootstrapperType DbBootstrapperType { get; set; }
    public Func<IServiceProvider, IDbBootstrapper>? DbBootstrapperFactory { get; set; }
    
    private readonly TestRuntimeOptionsBuilder _returnTo;

    public DbBootstrapperBuilderPart(TestRuntimeOptionsBuilder returnTo)
    {
        _returnTo = returnTo;
    }

    public TestRuntimeOptionsBuilder Use(Func<IServiceProvider, IDbBootstrapper> factory)
    {
        DbBootstrapperFactory = factory;
        return _returnTo;
    }
    
    public TestRuntimeOptionsBuilder UseFluentDocker(Func<IServiceProvider, FluentDockerOptions> action)
    {
        DbBootstrapperType = DbBootstrapperType.FluentDocker;
        DbBootstrapperFactory = provider =>
        {
            var options = action(provider);
            return new FluentDockerDbBootstrapper(options, provider.GetRequiredService<AppDbContext>());
        };
        return _returnTo;
    }

    public TestRuntimeOptionsBuilder UseExistingDb(Func<IServiceProvider, ExistingDbOptions> action)
    {
        DbBootstrapperType = DbBootstrapperType.ExistingDb;
        DbBootstrapperFactory = provider =>
        {
            var options = action(provider);
            return new ExistingDbBootstrapper(provider.GetRequiredService<AppDbContext>(), options);
        };
        return _returnTo;
    }
}