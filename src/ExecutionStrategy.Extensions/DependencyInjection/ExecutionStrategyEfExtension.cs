using EntityFrameworkCore.ExecutionStrategy.Extensions.Container;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions.DependencyInjection;

internal class ExecutionStrategyEfExtension: IDbContextOptionsExtension
{
    private readonly DependencyContainer _container;

    public ExecutionStrategyEfExtension(DependencyContainer container)
    {
        _container = container;
        Info = new ExecutionStrategyEfExtensionInfo(this);
    }

    public void ApplyServices(IServiceCollection services)
    {
        services.AddSingleton(_container);
    }

    public void Validate(IDbContextOptions options)
    {
    }

    public DbContextOptionsExtensionInfo Info { get; }

    private class ExecutionStrategyEfExtensionInfo : DbContextOptionsExtensionInfo
    {
        public ExecutionStrategyEfExtensionInfo(IDbContextOptionsExtension extension) : base(extension)
        {
        }

        public override int GetServiceProviderHashCode() => 0;
        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) =>
            other is ExecutionStrategyEfExtensionInfo;
        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) { }
        public override bool IsDatabaseProvider => false;
        public override string LogFragment => "";
    }
}