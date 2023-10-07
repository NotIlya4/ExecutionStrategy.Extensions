using ExecutionStrategyExtended.Demo.EntityFramework;
using ExecutionStrategyExtended.Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategyExtended.Demo.Services;

public class UserService
{
    private readonly AppDbContext _context;
    
    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddUser(User user)
    {
        _context.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is not null)
        {
            _context.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}