namespace ExecutionStrategy.Extensions.IntegrationTests;

public class Ticker
{
    private readonly int _reactOn;
    private readonly Action _action;
    private int _current = 0;

    public Ticker(int reactOn, Action action)
    {
        _reactOn = reactOn;
        _action = action;
    }

    public static Ticker CallOnFirstCall(Action action)
    {
        return new Ticker(1, action);
    }
    
    public static Ticker CallOnSecondCall(Action action)
    {
        return new Ticker(2, action);
    }

    public void Tick()
    {
        _current += 1;
        if (_current == _reactOn)
        {
            _action();
        }
    }
}

public class Thrower
{
    private readonly Ticker _ticker;

    public Thrower(int reactOn, Action action) : this(new Ticker(reactOn, action))
    {
        
    }
    
    public Thrower(Ticker ticker)
    {
        _ticker = ticker;
    }

    public static Thrower ThrowOnFirstCall()
    {
        return new Thrower(1, ThrowTransientException);
    }
    
    public static Thrower ThrowOnSecondCall()
    {
        return new Thrower(2, ThrowTransientException);
    }

    public void Tick()
    {
        _ticker.Tick();
    }

    public static void ThrowTransientException()
    {
        throw new TimeoutException();
    }
}