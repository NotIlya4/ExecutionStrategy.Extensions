using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategyExtended.DbContextRetrier.Implementations;

internal class CreateNewDbContextRetryBehavior<TDbContext> : IDbContextRetryBehavior<TDbContext> where TDbContext : DbContext
{
    private readonly bool _disposePreviousContext;
    private readonly IDbContextFactory<TDbContext> _factory;
    private readonly TDbContext _mainContext;
    private TDbContext? _previousContext;

    public CreateNewDbContextRetryBehavior(bool disposePreviousContext, IDbContextFactory<TDbContext> factory, TDbContext mainContext)
    {
        _disposePreviousContext = disposePreviousContext;
        _factory = factory;
        _mainContext = mainContext;
    }

    public IExecutionStrategy CreateExecutionStrategy()
    {
        return _mainContext.Database.CreateExecutionStrategy();
    }

    public async Task<TDbContext> ProvideDbContextForRetry(int attempt)
    {
        await DisposePreviousContext();

        var context = await _factory.CreateDbContextAsync();
        _previousContext = context;

        return context;
    }

    private async Task DisposePreviousContext()
    {
        if (_disposePreviousContext)
        {
            if (_previousContext is null)
            {
                return;
            }
            
            await _previousContext.DisposeAsync();
        }

        _previousContext = null;
    }
}