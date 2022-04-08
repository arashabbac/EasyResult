using EasyResult.Configurations;
using EasyResult.Utility;
using Microsoft.Extensions.Options;

namespace EasyResult;

public class Result
{
    protected readonly IOptions<ResultOptions> _options;

    public Result(IOptions<ResultOptions> options)
    {
        _options = options;
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

    public virtual Result WithSuccess(string message = default!)
    {
        IsSuccess = true;
        Errors.Clear();

        message = message.Fix()!;

        if (message is null) message = _options.Value.SuccessDefaultMessage;

        if (Successes.Contains(message) == false)
        {
            Successes.Add(message);
        }

        return this;
    }
}

public class Result<TData> : Result where TData : class
{
    public TData? Data { get; set; }
   
    public Result(IOptions<ResultOptions>? options): base(options!)
    {
    }

    public override Result<TData> WithSuccess(string message = default!)
    {
        IsSuccess = true;
        Errors.Clear();

        if (message is null) message = _options.Value.SuccessDefaultMessage;

        if (Successes.Contains(message) == false)
        {
            Successes.Add(message);
        }

        return this;
    }

    public Result<TData> WithData(TData data)
    {
        ArgumentNullException.ThrowIfNull(data,nameof(data));

        IsSuccess = true;
        Errors.Clear();

        Data = data;
        return this;
    }
}
