namespace ResultHandler.Utility;

public static class ResultExtension
{
    internal static Result ToResult(this object? value)
    {
        return value is null ?
            throw new ArgumentNullException(nameof(value)) : new Result<object>().WithData(value);
    }

    internal static Result ToResult(string message = "عمیات با موفقیت انجام شد")
    {
        var result = new Result();
        result.WithSuccess(message);
        return result;
    }

    internal static Result ToResult(this Exception ex)
    {
        var result = new Result();

        result.WithError(ex.Message);
        result.WithError(ex.GetException());
        return result;
    }
}
