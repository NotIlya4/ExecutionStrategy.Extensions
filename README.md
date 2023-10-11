# 🔄 ExecutionStrategy.Extensions
ExecutionStrategy.Extensions is a little convenient wrapper for `IExecutionStrategy` that simplifies work with `IExecutionStrategy`.

## Getting started
Add this to your `DbContextOptionsBuilder`:
```csharp
// Here might be any provider you use with retry on failure enabled
builder.UseNpgsql(conn, builder => 
    builder.EnableRetryOnFailure());

// Configure ExecutionStrategyExtended
builder.UseExecutionStrategyExtensions<AppDbContext>(
    builder => builder.WithClearChangeTrackerOnRetry());
```

Once you've configured it, you can use it inside your controllers like this:
```csharp
await context.ExecuteExtendedAsync(async () =>
{
    await service.AddUser(user);
});
```

This is equivalent to the following manual approach:
```csharp
var strategy = context.Database.CreateExecutionStrategy();
await strategy.ExecuteAsync(async () =>
{
    context.ChangeTracker.Clear();
    await service.AddUser(user);
});
```

## Why Clear the Change Tracker
The [Microsoft documentation](https://learn.microsoft.com/en-us/ef/ef6/fundamentals/connection-resiliency/retry-logic#solution-manually-call-execution-strategy) recommends recreating a new `DbContext` on each retry since otherwise it could lead to those bugs:
```csharp
strategy.ExecuteAsync(
	async (context) =>
	{
		var user = new User(0, "asd");

		context.Add(user);

		// Transient exception could occure here and IExecutionStrategy will retry execution 
		// It will lead to adding a second user to change tracker of DbContext
		var products = await context.Products.ToListAsync();

		await context.SaveChangesAsync();
	});
```
However, manually recreating `DbContext` in every retry can be inconvenient since you need to recreate instances of services to provide them new `DbContext`, instead you can clear change tracker on existing `DbContext` and reuse it.

## Transactions
You can manage transactions yourself or by using extension method on action builder:
```csharp
await context.ExecuteExtendedAsync(async () =>
{
    context.Users.Add(new User(0, "asd"));
    await context.SaveChangesAsync();
}, builder =>
{
    builder.WithTransaction(IsolationLevel.Serializable);
});
```

## Middlewares
If you need to customize the behavior of `WithClearChangeTrackerOnRetry`, you can do so by providing custom middleware in the builder. In fact, `WithTransaction` works on top of these middlewares too. Here's how `WithClearChangeTrackerOnRetry` written internally:
```csharp
builder.WithMiddleware(async (next, args) =>
{
    args.Context.ChangeTracker.Clear();
    return await next(args);
});
```

## Default options
Options provided inside `DbContextOptionsBuilder` are considered as defaults and will be applied for each execution. Besides `WithClearChangeTrackerOnRetry`, you can provide any middleware to customize behavior within each `context.ExecuteExtendedAsync`.