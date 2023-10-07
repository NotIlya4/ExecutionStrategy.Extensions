using ExecutionStrategy.Extensions.IntegrationTests.DbContextConfigurator;

namespace ExecutionStrategy.Extensions.IntegrationTests.PostgresBootstrapping.ExistingDb;

public class ExistingDbBootstrapper : IDbBootstrapper
{
    private readonly AppDbContext _context;
    private readonly ExistingDbOptions _options;

    public ExistingDbBootstrapper(AppDbContext context, ExistingDbOptions options)
    {
        _context = context;
        _options = options;
    }

    public async Task Bootstrap()
    {
        switch (_options.OnBootstrap)
        {
            case ExistingDbOptions.OnBootstrapType.RecreateDb:
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.EnsureCreatedAsync();
                break;
            case ExistingDbOptions.OnBootstrapType.CleanTables:
                await _context.CleanTables();
                break;
            case ExistingDbOptions.OnBootstrapType.DoNothing:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task Destroy()
    {
        switch (_options.OnDestroy)
        {
            case ExistingDbOptions.OnDestroyType.DropDb:
                await _context.Database.EnsureDeletedAsync();
                break;
            case ExistingDbOptions.OnDestroyType.CleanTables:
                await _context.CleanTables();
                break;
            case ExistingDbOptions.OnDestroyType.DoNothing:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task Clean()
    {
        switch (_options.OnClean)
        {
            case ExistingDbOptions.OnCleanType.RecreateDb:
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.EnsureCreatedAsync();
                break;
            case ExistingDbOptions.OnCleanType.CleanTables:
                await _context.CleanTables();
                break;
            case ExistingDbOptions.OnCleanType.DoNothing:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await Destroy();
    }
}