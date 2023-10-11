namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

/// <summary>
/// Builder with data.
/// </summary>
public interface IBuilderWithData
{
    /// <summary>
    /// Custom data that can be accessed anytime.
    /// </summary>
    IExecutionStrategyData Data { get; }
}

/// <summary>
/// <see cref="IBuilderWithData"/> extensions.
/// </summary>
public static class BuilderWithDataExtensions
{
    /// <summary>
    /// Provides custom data that can be accessed anytime.
    /// </summary>
    /// <param name="builder">Builder.</param>
    /// <param name="action">Options builder action.</param>
    /// <typeparam name="TReturn">Builder type.</typeparam>
    /// <returns>Same builder.</returns>
    public static TReturn WithData<TReturn>(this TReturn builder, Action<IExecutionStrategyData> action)
        where TReturn : IBuilderWithData
    {
        action(builder.Data);
        return builder;
    }
}