using ResultHandler.Exceptions;
using System.Net;

namespace ResultHandler.Services;

public class ExceptionService : IExceptionService
{
    private readonly List<ExceptionDto> _exceptions;
    public IReadOnlyList<ExceptionDto> Exceptions => _exceptions;
    public ExceptionService()
    {
        _exceptions = new();
        _exceptions.AddRange(new[] 
        { 
            new ExceptionDto(typeof(UnauthorizedAccessException),HttpStatusCode.Unauthorized),
            new ExceptionDto(typeof(NotFoundException),HttpStatusCode.NotFound),
            new ExceptionDto(typeof(BadRequestException),HttpStatusCode.BadRequest)
        });
    }
    public void AddException(ExceptionDto exception)
    {
        if(exception is null)
            throw new ArgumentNullException(nameof(exception));

        if (exception.ExceptionType.IsSubclassOf(typeof(Exception)) == false)
            throw new ArgumentException("Only exception type is allowed!");

        if (_exceptions.Any(c => c.ExceptionType == exception.ExceptionType))
            throw new DuplicateWaitObjectException("This exception type has already defined!");

        _exceptions.Add(exception);
    }

    public HttpStatusCode GetHttpStatusCodeByExceptionType(Exception exception)
    {
        var ex = _exceptions.FirstOrDefault(x => x.ExceptionType == exception.GetType());

        if (ex is not null) return ex.StatusCode;

        return HttpStatusCode.InternalServerError;
    }

    public List<ExceptionDto> GetExceptions() => _exceptions;
}
