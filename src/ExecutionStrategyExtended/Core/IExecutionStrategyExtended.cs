using System.Data;
using EntityFrameworkCore.ExecutionStrategyExtended.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ExecutionStrategyExtended.Core;

public interface IExecutionStrategyExtended<TDbContext> where TDbContext : DbContext
{
    IExecutionStrategyExtendedOptionsBuilder OptionsBuilder { get; }
    TDbContext MainContext { get; }
    
    Task<TResponse> ExecuteAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext);

    Task<TResponse> ExecuteInTransactionAsync<TResponse>(Func<TDbContext, Task<TResponse>> action,
        TDbContext mainContext, IsolationLevel isolationLevel);
}