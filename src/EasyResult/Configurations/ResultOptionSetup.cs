namespace EasyResult.Configurations;

internal static class ResultOptionSetup
{
    public static ResultOptions? Options { get; set; }
   

    public static void Setup(Action<ResultOptions>? action)
    {
        action ??= _ => new ResultOptions();
        Options = new ResultOptions();
        action.Invoke(Options);
    }
}
