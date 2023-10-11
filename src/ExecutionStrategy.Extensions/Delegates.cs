using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <summary>
/// Operation delegate without response
/// </summary>
public delegate Task ExecutionStrategyVoidNext();

/// <summary>
/// Operation delegate without response
/// </summary>
/// <typeparam name="TDbContext">Your DbContext type</typeparam>
public delegate Task ExecutionStrategyVoidNext<in TDbContext>(IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

/// <summary>
/// Operation delegate
/// </summary>
/// <typeparam name="TResult">Type of operation result</typeparam>
public delegate Task<TResult> ExecutionStrategyNext<TResult>();

/// <summary>
/// Operation delegate
/// </summary>
/// <typeparam name="TDbContext">Your DbContext type</typeparam>
/// <typeparam name="TResult">Type of operation result</typeparam>
public delegate Task<TResult> ExecutionStrategyNext<in TDbContext, TResult>(
    IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

/// <summary>
/// Middleware delegate with empty response
/// </summary>
/// <typeparam name="TDbContext">Your DbContext type</typeparam>
public delegate Task ExecutionStrategyVoidMiddleware<TDbContext>(
    ExecutionStrategyVoidNext<TDbContext> next, IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

/// <summary>
/// Middleware delegate
/// </summary>
/// <typeparam name="TDbContext">Your DbContext type</typeparam>
/// <typeparam name="TResult">Type of operation result</typeparam>
public delegate Task<TResult> ExecutionStrategyMiddleware<TDbContext, TResult>(
    ExecutionStrategyNext<TDbContext, TResult> next, IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;