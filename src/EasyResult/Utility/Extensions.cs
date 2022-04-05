namespace EasyResult.Utility;

public static class Extensions
{
    public static string GetException(this Exception exception)
    {
        var exceptionMessage = exception.Message + "\r\n";
        while (exception.InnerException != null)
        {
            exception = exception.InnerException;
            exceptionMessage += exception.Message + "\r\n";
        }

        return exceptionMessage;
    }

    public static string? Fix(this string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        
        text = text.Trim();

        while (text.Contains("  "))
        {
            text = text.Replace("  ", " ");
        }

        return text;
    }
}
