using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public delegate Task ExecutionStrategyVoidOperation();

public delegate Task ExecutionStrategyVoidOperation<in TDbContext>(IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

public delegate Task<TResult> ExecutionStrategyOperation<TResult>();

public delegate Task<TResult> ExecutionStrategyOperation<in TDbContext, TResult>(
    IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

public delegate Task ExecutionStrategyVoidMiddleware<TDbContext>(
    ExecutionStrategyVoidOperation<TDbContext> next, IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

public delegate Task<TResult> ExecutionStrategyMiddleware<TDbContext, TResult>(
    ExecutionStrategyOperation<TDbContext, TResult> next, IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;