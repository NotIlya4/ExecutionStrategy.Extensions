namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <summary>
/// <see cref="IExecutionStrategyData"/> extensions.
/// </summary>
public static class DataExtensions
{
    /// <summary>
    /// Set data by its type name.
    /// </summary>
    /// <param name="dictionary">Data</param>
    /// <param name="value">Value</param>
    /// <typeparam name="T">Value type</typeparam>
    public static void Set<T>(this IExecutionStrategyData dictionary, T value)
    {
        dictionary[typeof(T).Name] = value!;
    }
    
    /// <summary>
    /// Get data by its type name.
    /// </summary>
    /// <param name="dictionary">Data</param>
    /// <typeparam name="T">Value type</typeparam>
    /// <returns></returns>
    public static T Get<T>(this IExecutionStrategyData dictionary)
    {
        return (T)dictionary[typeof(T).Name];
    }

    /// <summary>
    /// Try get data by its type name.
    /// </summary>
    /// <param name="dictionary">Data</param>
    /// <typeparam name="T">Value type</typeparam>
    /// <returns>Value</returns>
    public static T? TryGet<T>(this IExecutionStrategyData dictionary)
    {
        if (dictionary.TryGetValue(typeof(T).Name, out var ret))
        {
            return (T)ret;
        }

        return default;
    }
    
    /// <summary>
    /// Set data under provided key.
    /// </summary>
    /// <param name="dictionary">Data</param>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <typeparam name="T">Value type</typeparam>
    public static void Set<T>(this IExecutionStrategyData dictionary, string key, T value)
    {
        dictionary[key] = value!;
    }
    
    /// <summary>
    /// Get data under key.
    /// </summary>
    /// <param name="dictionary">Data</param>
    /// <param name="key">Key</param>
    /// <typeparam name="T">Value type</typeparam>
    /// <returns>Value</returns>
    public static T Get<T>(this IExecutionStrategyData dictionary, string key)
    {
        return (T)dictionary[key];
    }
}