using FluentValidation;
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

        if (ex is ValidationException)
        {
            foreach (var error in ((ValidationException)ex).Errors)
                result.WithError(error.ErrorMessage);
        }

        result.WithError(ex.Message);
        result.WithError(ex.GetException());
        return result;
    }

    //public static Result ToResult(this ValidationException ex)
    //{
    //    var result = new Result();

    //    foreach (var error in ex.Errors)
    //        result.WithError(error.ErrorMessage);

    //    return result;
    //}
}
