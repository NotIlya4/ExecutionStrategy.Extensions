# 🔄 ExecutionStrategyExtended
This is just a little wrapper around `IExecutionStrategy` that implements different approaches to work with regular `IExecutionStrategy` such as creating a new `DbContext` on retry or use same `DbContext` between retries but clear its change tracker.

## What this package solves