using EntityFrameworkCore.ExecutionStrategy.Extensions;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExecutionStrategy.Extensions.IntegrationTests.Tests;


[Collection("default")]
public class UnitTest1 : TestBase
{
    private readonly AppDbContext _context;

    public UnitTest1(TestFixture fixture) : base(fixture)
    {
        _context = fixture.Services.GetRequiredService<AppDbContext>();
    }

    [Fact]
    public async Task Test4()
    {
        bool isThrown = false;

        await _context.ExecuteExtendedAsync(async () =>
        {
            _context.Users.Add(new User(0, "Biba", false));

            if (!isThrown)
            {
                isThrown = true;
                ThrowTimout();
            }

            await _context.SaveChangesAsync();
        });

        _context.Clear();

        await Verify(await _context.Users.SingleAsync());
    }
}