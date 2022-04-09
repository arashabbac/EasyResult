using EasyResult;
using EasyResult.Configurations;
using Microsoft.Extensions.Options;

namespace EasyResult.Utility;

public static class ResultExtension
{
    internal static Result ToResult(this object? value,IOptions<ResultOptions> options)
    {
        return new Result<object>().WithData(value!);
    }

    internal static Result ToResult(IOptions<ResultOptions> options,string message = default!)
    {
        var result = new Result();
        result.WithSuccess(message);
        return result;
    }

    internal static Result ToResult(this Exception ex, IOptions<ResultOptions> options)
    {
        var result = new Result();

        result.WithError(ex.Message);
        result.WithError(ex.GetException());
        return result;
    }
}
