namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <summary>
/// Void type for Operations with void return.
/// </summary>
public struct Void
{
    /// <summary>
    /// Singleton instance.
    /// </summary>
    public static Void Instance { get; } = new Void();
}