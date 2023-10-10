using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public interface IBuilderWithVerifySucceeded<out TDbContext, TResult, out TReturn> where TDbContext : DbContext
{
    TReturn WithVerifySucceeded(ExecutionStrategyNext<TDbContext, ExecutionResult<TResult>> verifySucceeded);
}

public static class BuilderWithVerifySucceededExtensions
{
    public static TReturn WithVerifySucceeded<TDbContext, TResult, TReturn>(
        this IBuilderWithVerifySucceeded<TDbContext, TResult, TReturn> builder,
        ExecutionStrategyNext<TDbContext, TResult?> verifySucceeded)
        where TReturn : IBuilderWithVerifySucceeded<TDbContext, TResult, TReturn> where TDbContext : DbContext
    {
        return builder.WithVerifySucceeded(async args =>
        {
            var result = await verifySucceeded(args);
            return new ExecutionResult<TResult>(result is not null, result!);
        });
    }
    
    public static TReturn WithVerifySucceeded<TDbContext, TReturn>(
        this IBuilderWithVerifySucceeded<TDbContext, Void, TReturn> builder,
        ExecutionStrategyNext<TDbContext, bool> verifySucceeded)
        where TReturn : IBuilderWithVerifySucceeded<TDbContext, Void, TReturn> where TDbContext : DbContext
    {
        return builder.WithVerifySucceeded(async args =>
        {
            var result = await verifySucceeded(args);
            return new ExecutionResult<Void>(result, Void.Instance);
        });
    }
}