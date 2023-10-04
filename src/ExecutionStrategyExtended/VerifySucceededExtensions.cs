using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

public static class VerifySucceededExtensions
{
    public static Func<TDbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>> VerifySucceeded<TDbContext, TState, TResult, TReturn>(this TReturn data)
        where TReturn : IWithVerifySucceeded, IDictionary<object, object>
    {
        return (Func<TDbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>>)data["VerifySucceeded"];
    }
    
    public static void VerifySucceeded<TDbContext, TState, TResult, TReturn>(this TReturn data,
        Func<TDbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>> verifySucceeded)
        where TReturn : IWithVerifySucceeded, IDictionary<object, object>
    {
        data["VerifySucceeded"] = verifySucceeded;
    }
    
    public static TReturn WithVerifySucceeded<TDbContext, TState, TResult, TReturn>(this TReturn data,
        Func<TDbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>> verifySucceeded)
        where TReturn : IWithVerifySucceeded, IDictionary<object, object>
    {
        data.VerifySucceeded(verifySucceeded);
        return data;
    }
}

public interface IWithVerifySucceeded
{
    
}