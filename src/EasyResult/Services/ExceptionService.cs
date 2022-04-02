using System.Net;

namespace EasyResult.Services;

public class ExceptionService
{
    private readonly Dictionary<Type, HttpStatusCode> _exceptions;
    public IReadOnlyDictionary<Type, HttpStatusCode> Exceptions => _exceptions;
    public ExceptionService()
    {
        _exceptions = new();
    }

    public HttpStatusCode GetHttpStatusCodeByExceptionType(Exception exception)
    {
        var ex = _exceptions.FirstOrDefault(x => x.Key == exception.GetType());

        if (ex.Key is not null) return ex.Value;

        return HttpStatusCode.InternalServerError;
    }

    public IReadOnlyDictionary<Type, HttpStatusCode> GetExceptions() => Exceptions;

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
