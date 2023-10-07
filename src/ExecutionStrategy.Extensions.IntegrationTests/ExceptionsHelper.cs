namespace ExecutionStrategy.Extensions.IntegrationTests;

public static class ExceptionsHelper
{
    public static void ThrowTimout()
    {
        throw new TimeoutException();
    }
}