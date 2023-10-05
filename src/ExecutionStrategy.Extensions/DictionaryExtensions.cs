namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public static class DictionaryExtensions
{
    public static void Set<T>(this IExecutionStrategyData dictionary, T value)
    {
        dictionary[typeof(T).Name] = value!;
    }
    
    public static T Get<T>(this IExecutionStrategyData dictionary)
    {
        return (T)dictionary[typeof(T).Name];
    }

    public static T? TryGet<T>(this IExecutionStrategyData dictionary)
    {
        if (dictionary.TryGetValue(typeof(T).Name, out var ret))
        {
            return (T)ret;
        }

        return default;
    }
    
    public static void Set<T>(this IExecutionStrategyData dictionary, string key, T value)
    {
        dictionary[key] = value!;
    }
    
    public static T Get<T>(this IExecutionStrategyData dictionary, string key)
    {
        return (T)dictionary[key];
    }
}