using EasyResult.Configurations;
using EasyResult.Utility;

namespace EasyResult;

public class Result
{
    private readonly ResultOptions? _options;
    protected ResultOptions? Options => _options;
    public Result()
    {
        _options = ResultOptionSetup.Options;
        Errors = new List<string>();
        Successes = new List<string>();
    }

    /// <summary>
    /// Represents whether the operation is successful or not
    /// </summary>
    public bool IsSuccess { get; set; }
    
    /// <summary>
    /// List of error messsages
    /// </summary>
    public List<string> Errors { get; set; }
    
    /// <summary>
    /// List of success messages
    /// </summary>
    public List<string> Successes { get; set; }

    /// <summary>
    /// Add an error message to result object
    /// </summary>
    /// <param name="errorMessage">Erroe message</param>
    /// <returns></returns>
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

    /// <summary>
    /// Retrun result object with error and make it unsuccessful
    /// </summary>
    /// <returns></returns>
    public Result WithError()
    {
        IsSuccess = false;
        Successes.Clear();
        return this;
    }

    /// <summary>
    /// Add a success message to result object
    /// </summary>
    /// <param name="message">message</param>
    /// <returns></returns>
    public virtual Result WithSuccess(string message = default!)
    {
        IsSuccess = true;
        Errors.Clear();

        message = message.Fix()!;

        if (message is null && Options is not null) 
            message = Options.SuccessDefaultMessage;

        if (Successes.Contains(message!) == false)
        {
            Successes.Add(message!);
        }

        return this;
    }

    /// <summary>
    /// Retrun ResultOptions
    /// </summary>
    /// <returns></returns>
    public ResultOptions? GetOption() => _options;
}

public class Result<TData> : Result where TData : class
{
    public TData? Data { get; set; }

    public Result() 
    {

    }

    /// <summary>
    /// Add a success message to result object
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public override Result<TData> WithSuccess(string message = default!)
    {
        IsSuccess = true;
        Errors.Clear();

        if (message is null && Options is not null) 
            message = Options.SuccessDefaultMessage;

        if (Successes.Contains(message!) == false)
        {
            Successes.Add(message!);
        }

        return this;
    }

    /// <summary>
    /// Add data object into result object
    /// </summary>
    /// <param name="data">object to store in Data property</param>
    /// <returns></returns>
    public Result<TData> WithData(TData data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        IsSuccess = true;
        Errors.Clear();

        Data = data;
        return this;
    }
}

