using System.Data;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public interface IExecutionStrategyExtended<TDbContext> where TDbContext : DbContext
{
    Task<TResponse> ExecuteAsync<TResponse>(Func<TDbContext, Task<TResponse>> action);

    Task<TResponse> ExecuteInTransactionAsync<TResponse>(Func<TDbContext, Task<TResponse>> action, IsolationLevel isolationLevel);
}