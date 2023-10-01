using EntityFrameworkCore.ExecutionStrategyExtended.Core;
using ExecutionStrategyExtended.Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.Demo;

public class UserService
{
    private readonly IActualDbContextProvider<AppDbContext> _context;
    
    public UserService(IActualDbContextProvider<AppDbContext> context)
    {
        _context = context;
    }

    public async Task AddUser(User user)
    {
        _context.DbContext.Add(user);
        await _context.DbContext.SaveChangesAsync();
    }

    public async Task DeleteUser(int userId)
    {
        var user = await _context.DbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is not null)
        {
            _context.DbContext.Remove(user);
            await _context.DbContext.SaveChangesAsync();
        }
    }
}