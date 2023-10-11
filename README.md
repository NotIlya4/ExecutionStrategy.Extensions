# 🔄 ExecutionStrategyExtended
Just a bunch of extensions for `IExecutionStrategy`.

## Getting started
Add this to your `DbContextOptionsBuilder`:
```csharp
// Here might be any provider you use with retry on failure enabled
builder.UseNpgsql(conn, builder => 
    builder.EnableRetryOnFailure());

builder.UseExecutionStrategyExtensions<AppDbContext>(
    builder => builder.WithClearChangeTrackerOnRetry());
```

And use it inside your controllers like this:
```csharp
await context.ExecuteExtendedAsync(async () =>
{
    await service.AddUser(user);
});
```

This is equivalent to:
```csharp
var strategy = context.Database.CreateExecutionStrategy();
await strategy.ExecuteAsync(async () =>
{
    context.ChangeTracker.Clear();
    await service.AddUser(user);
});
```

## Why am i clearing change tracker
Microsoft [documentation](https://learn.microsoft.com/en-us/ef/ef6/fundamentals/connection-resiliency/retry-logic#solution-manually-call-execution-strategy) suggests you to recreate a new `DbContext` on each retry which is true because consider this example:
```csharp
strategy.ExecuteAsync(
	async (context) =>
	{
		var user = new User(0, "asd");

		context.Add(user);

		// Transient exception could occure here and IExecutionStrategy will retry execution 
		// and it will lead to adding a second user to change tracker of DbContext
		var products = await context.Products.ToListAsync();

		await context.SaveChangesAsync();
	});
```
And to avoid it you need to recreate `DbContext` but it might be inconvenient in most of the cases because you need to recreate a new instances of services to provide them new `DbContext`, instead you can clear change tracker on existing `DbContext` and reuse it.

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
If you need to customize `WithClearChangeTrackerOnRetry` behavior you could do so by providing custom middleware in builder, actually `WithTransaction` works on top of middlewares too:
```csharp
builder.WithMiddleware(async (next, args) =>
{
    args.Context.ChangeTracker.Clear();
    return await next(args);
});
```

## Default options
Options provided inside `DbContextOptionsBuilder` considered as default and will be applied for each execution. Besides `WithClearChangeTrackerOnRetry` you can provide any middleware to it and it will be called in each `context.ExecuteExtendedAsync`.