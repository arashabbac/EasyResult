using EasyResult.Utility;

namespace EasyResult;

public class Result
{
    public Result()
    {
        Errors = new List<string>();
        Successes = new List<string>();
    }
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; }
    public List<string> Successes { get; set; }

    public Result WithError(string errorMessage)
    {
        IsSuccess = false;
        Successes.Clear();

        errorMessage = errorMessage.Fix()!;

        if (string.IsNullOrEmpty(errorMessage) == false &&
            Errors.Contains(errorMessage) == false)
        {
            Errors.Add(errorMessage);
        }

        return this;
    }

    public Result WithError()
    {
        IsSuccess = false;
        Successes.Clear();
        return this;
    }

    public Result WithSuccess(string message = "Operation has been done successfully!")
    {
        IsSuccess = true;
        Errors.Clear();

        message = message.Fix()!;

        if (string.IsNullOrEmpty(message) == false &&
            Successes.Contains(message) == false)
        {
            Successes.Add(message);
        }

        return this;
    }
}

public class Result<TData> : Result where TData : class
{
    public TData? Data { get; set; }

    public Result<TData> WithData(TData data, string message = "Operation has been done successfully!")
    {
        WithSuccess(message);
        Data = data;
        return this;
    }
}
