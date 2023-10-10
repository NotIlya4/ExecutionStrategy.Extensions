namespace EntityFrameworkCore.ExecutionStrategy.Extensions;

public interface IBuilderWithData
{
    IExecutionStrategyData Data { get; }
}

public static class BuilderWithDataExtensions
{
    public static TReturn WithData<TReturn>(this TReturn returnTo, Action<IExecutionStrategyData> action)
        where TReturn : IBuilderWithData
    {
        action(returnTo.Data);
        return returnTo;
    }
}