using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public static class VoidExtensions
{
    public static Task ExecuteExtendedAsync<TDbContext>(this TDbContext context,
        ExecutionStrategyVoidNext<TDbContext> operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, Void>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(async args =>
        {
            await operation(args);
            return new Void();
        }, action);
    }
    
    public static Task ExecuteExtendedAsync<TDbContext>(this TDbContext context,
        ExecutionStrategyVoidNext operation,
        Action<IExecutionStrategyOptionsBuilder<TDbContext, Void>>? action = null) where TDbContext : DbContext
    {
        return context.ExecuteExtendedAsync(_ => operation(), action);
    }
    
    public static IExecutionStrategyOptionsBuilder<TDbContext, Void> WithVerifySucceeded<TDbContext>(
        this IExecutionStrategyOptionsBuilder<TDbContext, Void> builder, ExecutionStrategyNext<TDbContext, bool> verifySucceeded) where TDbContext : DbContext
    {
        return builder.WithVerifySucceeded(async args => new ExecutionResult<Void>(await verifySucceeded(args), new Void()));
    }
}