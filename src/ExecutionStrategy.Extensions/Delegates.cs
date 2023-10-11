using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <summary>
/// Operation delegate without response.
/// </summary>
public delegate Task ExecutionStrategyVoidNext();

/// <summary>
/// Operation delegate without response.
/// </summary>
/// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
public delegate Task ExecutionStrategyVoidNext<in TDbContext>(IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

/// <summary>
/// Operation delegate.
/// </summary>
/// <typeparam name="TResult">Return type of your operation.</typeparam>
public delegate Task<TResult> ExecutionStrategyNext<TResult>();

/// <summary>
/// Operation delegate.
/// </summary>
/// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
/// <typeparam name="TResult">Return type of your operation.</typeparam>
public delegate Task<TResult> ExecutionStrategyNext<in TDbContext, TResult>(
    IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

/// <summary>
/// Middleware delegate without response.
/// </summary>
/// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
public delegate Task ExecutionStrategyVoidMiddleware<TDbContext>(
    ExecutionStrategyVoidNext<TDbContext> next, IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

/// <summary>
/// Middleware delegate.
/// </summary>
/// <typeparam name="TDbContext">Type of your <see cref="DbContext"/>.</typeparam>
/// <typeparam name="TResult">Return type of your operation.</typeparam>
public delegate Task<TResult> ExecutionStrategyMiddleware<TDbContext, TResult>(
    ExecutionStrategyNext<TDbContext, TResult> next, IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;