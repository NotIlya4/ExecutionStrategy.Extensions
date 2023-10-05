## Usage with options override
```csharp
context.ExecuteExtendedAsync<AppDbContext, bool, List<User>>(
    async (dbContext, state, cancellationToken) =>
    {
        // Some usage of dbContext
        return await dbContext.Users.ToListAsync();
    },
    builder =>
    {
        builder
            .WithCancellationToken(cancellationToken)
            .WithState(state)
            .WithVerifySucceeded(verifySucceeded)
            .WithDbContextOnRetryProvider(args => args.PreviousDbContext);
    });
```

```csharp
context.ExecuteExtendedAsync(
    async (dbContext, cancellationToken) =>
    {
        // Some usage of dbContext
        return await dbContext.Users.ToListAsync();
    },
    builder =>
    {
        builder
            .WithCancellationToken(cancellationToken)
            .WithVerifySucceeded(verifySucceeded)
            .WithDbContextOnRetryProvider(args =>
            {
                args.MainDbContext.ChangeTracker.Clear();
                return args.MainDbContext;
            });
    });
```

```csharp
services.AddExecutionStrategyExtensions();
```

```csharp
builder.UseExecutionStrategyExtensions(optionsBuilder =>
{
    optionsBuilder
        .CleanChangeTrackerOnRetry()
        .WithDbContextOnRetryProvider(args =>
        {
            args.MainDbContext.ChangeTracker.Clear();
            return args.MainDbContext;
        })
        .WithActualDbContextProvider(_actualDbConextProvider);
});
```