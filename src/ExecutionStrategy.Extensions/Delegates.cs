using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public delegate Task ExecutionStrategyVoidNext();

public delegate Task ExecutionStrategyVoidNext<in TDbContext>(IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

public delegate Task<TResult> ExecutionStrategyNext<TResult>();

public delegate Task<TResult> ExecutionStrategyNext<in TDbContext, TResult>(
    IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

public delegate Task ExecutionStrategyVoidMiddleware<TDbContext>(
    ExecutionStrategyVoidNext<TDbContext> next, IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

public delegate Task<TResult> ExecutionStrategyMiddleware<TDbContext, TResult>(
    ExecutionStrategyNext<TDbContext, TResult> next, IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;