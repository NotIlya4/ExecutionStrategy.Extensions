using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ExecutionStrategyExtended;

// public static class ExecutionStrategyExtensions
// {
//     public static async Task<TResult> ExecuteAsync<TDbContext, TState, TResult>(
//         this IExecutionStrategy strategy,
//         TState state,
//         Func<TDbContext, TState, CancellationToken, Task<TResult>> operation,
//         Func<TDbContext, TState, CancellationToken, Task<ExecutionResult<TResult>>>? verifySucceeded,
//         CancellationToken cancellationToken = default) where TDbContext : DbContext
//     {
//         return await strategy.ExecuteAsync(
//             state,
//             async (actionContext, actionState, actionCancellationToken) =>
//                 await operation((TDbContext)actionContext, actionState, actionCancellationToken),
//             verifySucceeded is not null
//                 ? async (actionContext, actionState, actionCancellationToken) =>
//                     await verifySucceeded((TDbContext)actionContext, actionState, actionCancellationToken)
//                 : null,
//             cancellationToken);
//     }
//
//     public static TResult Execute<TDbContext, TState, TResult>(
//         this IExecutionStrategy strategy,
//         TState state,
//         Func<TDbContext, TState, TResult> operation,
//         Func<TDbContext, TState, ExecutionResult<TResult>>? verifySucceeded) where TDbContext : DbContext
//     {
//         return strategy.Execute(
//             state,
//             (actionContext, actionState) => operation((TDbContext)actionContext, actionState),
//             verifySucceeded is not null
//                 ? (actionContext, actionState) => verifySucceeded((TDbContext)actionContext, actionState)
//                 : null);
//     }
//
//     public static async Task<TResult> ExecuteAsync<TDbContext, TResult>(
//         this IExecutionStrategy strategy,
//         Func<TDbContext, CancellationToken, Task<TResult>> operation,
//         CancellationToken cancellationToken = default) where TDbContext : DbContext
//     {
//         return await strategy.ExecuteAsync<TDbContext, bool, TResult>(
//             true,
//             async (actionContext, _, actionCancellationToken) =>
//                 await operation(actionContext, actionCancellationToken),
//             null,
//             cancellationToken);
//     }
//
//     public static async Task<TResult> ExecuteAsync<TDbContext, TResult>(
//         this IExecutionStrategy strategy,
//         Func<TDbContext, Task<TResult>> operation) where TDbContext : DbContext
//     {
//         return await strategy.ExecuteAsync<TDbContext, TResult>(async (actionContext, _) =>
//             await operation(actionContext));
//     }
//
//     public static TResult Execute<TDbContext, TResult>(
//         this IExecutionStrategy strategy,
//         Func<TDbContext, TResult> operation) where TDbContext : DbContext
//     {
//         return strategy.Execute<TDbContext, bool, TResult>(
//             true,
//             (actionContext, _) => operation(actionContext),
//             null);
//     }
// }