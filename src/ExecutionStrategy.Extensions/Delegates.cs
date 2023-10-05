using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public delegate TResult ExecutionStrategyOperation<TDbContext, out TResult>(IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;

public delegate TResult ExecutionStrategyMiddleware<TDbContext, TResult>(
    ExecutionStrategyOperation<TDbContext, TResult> next, IExecutionStrategyOperationArgs<TDbContext> args)
    where TDbContext : DbContext;