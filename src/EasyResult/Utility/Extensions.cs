namespace EasyResult.Utility;

public static class Extensions
{
    public static string GetException(this Exception exception)
    {
        string ExceptionMessage = exception.Message + "\r\n";
        while (exception.InnerException != null)
        {
            exception = exception.InnerException;
            ExceptionMessage += exception.Message + "\r\n";
        }

        return ExceptionMessage;
    }

    public static string? Fix(this string text)
    {
        if (text is null)
        {
            return null;
        }

        text = text.Trim();

        if (text == string.Empty)
        {
            return null;
        }

        while (text.Contains("  "))
        {
            text = text.Replace("  ", " ");
        }

        return text;
    }
}
