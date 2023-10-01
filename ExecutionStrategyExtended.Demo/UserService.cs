using EntityFrameworkCore.ExecutionStrategyExtended.Core;

namespace ExecutionStrategyExtended.Demo;

public class UserService
{
    private readonly IActualDbContextProvider<AppDbContext> _context;


    public UserService(IActualDbContextProvider<AppDbContext> context)
    {
        _context = context;
    }
}