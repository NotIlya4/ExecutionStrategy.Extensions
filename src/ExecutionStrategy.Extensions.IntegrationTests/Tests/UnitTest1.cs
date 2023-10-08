using EntityFrameworkCore.ExecutionStrategy.Extensions;
using ExecutionStrategy.Extensions.IntegrationTests.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ExecutionStrategy.Extensions.IntegrationTests.Tests;

public class UnitTest1 : TestBase
{
    private readonly AppDbContext _context;

    public UnitTest1(TestFixture fixture) : base(fixture)
    {
        _context = fixture.AppDbContext();
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

        await Verify(await _context.Users.SingleAsync(), new VerifySettings());
    }
    
    [Fact]
    public async Task Test1()
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
    
    [Fact]
    public async Task Test2()
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

public class UnitTest2 : TestBase
{
    private readonly AppDbContext _context;

    public UnitTest2(TestFixture fixture) : base(fixture)
    {
        _context = fixture.AppDbContext();
    }
    
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
    
    [Fact]
    public async Task Test1()
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
    
    [Fact]
    public async Task Test2()
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

public class UnitTest3 : TestBase
{
    private readonly AppDbContext _context;

    public UnitTest3(TestFixture fixture) : base(fixture)
    {
        _context = fixture.AppDbContext();
    }
    
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
    
    [Fact]
    public async Task Test1()
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
    
    [Fact]
    public async Task Test2()
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

public class UnitTest4 : TestBase
{
    private readonly AppDbContext _context;

    public UnitTest4(TestFixture fixture) : base(fixture)
    {
        _context = fixture.AppDbContext();
    }
    
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
    
    [Fact]
    public async Task Test1()
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
    
    [Fact]
    public async Task Test2()
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