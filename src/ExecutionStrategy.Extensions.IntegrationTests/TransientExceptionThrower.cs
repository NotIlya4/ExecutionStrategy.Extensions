namespace ExecutionStrategy.Extensions.IntegrationTests;

public class TransientExceptionThrower
{
    public bool IsThrown { get; set; }
    
    public void ThrowOnlyOnce()
    {
        if (!IsThrown)
        {
            IsThrown = true;
            throw new TimeoutException();
        }
    }
}