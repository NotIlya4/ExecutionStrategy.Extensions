namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <summary>
/// Void type for non Result extensions
/// </summary>
public struct Void
{
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static Void Instance { get; } = new Void();
}