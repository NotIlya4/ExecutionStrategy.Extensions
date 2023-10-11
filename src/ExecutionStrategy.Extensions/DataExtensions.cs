namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <summary>
/// <see cref="IExecutionStrategyData"/> extensions.
/// </summary>
public static class DataExtensions
{
    /// <summary>
    /// Set data using its type name as a key.
    /// </summary>
    /// <param name="data">Data.</param>
    /// <param name="value">Value.</param>
    /// <typeparam name="T">Value type.</typeparam>
    public static void Set<T>(this IExecutionStrategyData data, T value)
    {
        data[typeof(T).Name] = value!;
    }
    
    /// <summary>
    /// Get data using its type name as a key.
    /// </summary>
    /// <param name="data">Data.</param>
    /// <typeparam name="T">Value type.</typeparam>
    /// <returns>Value.</returns>
    public static T Get<T>(this IExecutionStrategyData data)
    {
        return (T)data[typeof(T).Name];
    }

    /// <summary>
    /// Try get data using its type name as a key.
    /// </summary>
    /// <param name="data">Data.</param>
    /// <typeparam name="T">Value type.</typeparam>
    /// <returns>Value.</returns>
    public static T? TryGet<T>(this IExecutionStrategyData data)
    {
        if (data.TryGetValue(typeof(T).Name, out var ret))
        {
            return (T)ret;
        }

        return default;
    }
    
    /// <summary>
    /// Set data using provided key.
    /// </summary>
    /// <param name="data">Data.</param>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    /// <typeparam name="T">Value type.</typeparam>
    public static void Set<T>(this IExecutionStrategyData data, string key, T value)
    {
        data[key] = value!;
    }
    
    /// <summary>
    /// Get data using provided key.
    /// </summary>
    /// <param name="data">Data.</param>
    /// <param name="key">Key.</param>
    /// <typeparam name="T">Value type.</typeparam>
    /// <returns>Value.</returns>
    public static T Get<T>(this IExecutionStrategyData data, string key)
    {
        return (T)data[key];
    }
}