global using static ExecutionStrategy.Extensions.IntegrationTests.Helpers.ThrowTransientExceptionStatic;

namespace ExecutionStrategy.Extensions.IntegrationTests.Helpers;

public class Hooker
{
    private readonly Action _action;
    private readonly int _triggerOn;
    private int _inc = 0;

    public Hooker(Action action, int triggerOn)
    {
        _action = action;
        _triggerOn = triggerOn;
    }

    public static Hooker ThrowTransientOnce()
    {
        return new Hooker(ThrowTransientException, 1);
    }

    public void Trigger()
    {
        _inc += 1;
        if (_inc == _triggerOn)
        {
            _action();
        }
    }
}

public static class ThrowTransientExceptionStatic
{
    public static void ThrowTransientException()
    {
        throw new TimeoutException();
    }
}