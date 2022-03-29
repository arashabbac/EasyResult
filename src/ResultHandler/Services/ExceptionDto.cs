using System.Net;

namespace ResultHandler.Services;

public class ExceptionDto
{

    public ExceptionDto(Type exceptionType,HttpStatusCode statusCode)
    {
        ExceptionType = exceptionType;
        StatusCode = statusCode;
    }

    public Type ExceptionType { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}
