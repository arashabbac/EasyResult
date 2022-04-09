using EasyResult.Configurations;
using System.Net;

namespace EasyResult.Services;

public class ExceptionService
{
    private readonly Dictionary<Type, HttpStatusCode> _exceptions;
    public IReadOnlyDictionary<Type, HttpStatusCode> Exceptions => _exceptions;
    private readonly ResultOptions? _options;
    public ExceptionService()
    {
        _exceptions = new();
        _options = ResultOptionSetup.Options;
    }

    /// <summary>
    /// Get HttpStatusCode by exception type
    /// </summary>
    /// <param name="exception">exception</param>
    /// <returns>HttpStatusCode</returns>
    public HttpStatusCode GetHttpStatusCodeByException(Exception exception)
    {
        var ex = _exceptions.FirstOrDefault(x => x.Key == exception.GetType());

        if (ex.Key is not null) return ex.Value;

        return _options!.UnhandledExceptionStatusCode;
    }

    public IReadOnlyDictionary<Type, HttpStatusCode> GetExceptions() => Exceptions;

    /// <summary>
    /// Assign HttpStatusCode to Exception type
    /// </summary>
    /// <param name="exceptionType">Exception type</param>
    /// <param name="statusCode">HttpStatusCode</param>
    /// <exception cref="ArgumentNullException">Throws when exception type is null</exception>
    /// <exception cref="ArgumentException">Throws when object type is not exception</exception>
    /// <exception cref="DuplicateWaitObjectException">Throws when exception type has already defined</exception>
    public void Add(Type exceptionType, HttpStatusCode statusCode)
    {
        if (exceptionType is null)
            throw new ArgumentNullException(nameof(exceptionType));

        if (exceptionType.IsSubclassOf(typeof(Exception)) == false)
            throw new ArgumentException("Only exception type is allowed!");

        if (_exceptions.Any(c => c.Key == exceptionType))
            throw new DuplicateWaitObjectException("This exception type has already defined!");

        _exceptions.TryAdd(exceptionType, statusCode);
    }
}
