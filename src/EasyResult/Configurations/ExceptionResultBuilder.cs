using EasyResult.Services;
using System.Net;

namespace EasyResult.Configurations;

public class ExceptionResultBuilder<T> where T : class
{
    private readonly ExceptionService _exceptionService;

    public ExceptionResultBuilder(ExceptionService exceptionService)
    {
        _exceptionService = exceptionService;
    }

    public void WithHttpStatusCode(HttpStatusCode httpStatusCode)
    {
        _exceptionService.Add(typeof(T), httpStatusCode);
    }
}
