using System.Net;

namespace ResultHandler.Services;

public interface IExceptionService
{
    void AddException(ExceptionDto exception);
    HttpStatusCode GetHttpStatusCodeByExceptionType(Exception exception);
    List<ExceptionDto> GetExceptions();
}
