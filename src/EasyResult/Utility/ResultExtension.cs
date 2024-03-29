﻿namespace EasyResult.Utility;

public static class ResultExtension
{
    internal static Result ToResult(this object? value)
    {
        return new Result<object>().WithData(value!);
    }

    internal static Result ToResult(string message = default!)
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
