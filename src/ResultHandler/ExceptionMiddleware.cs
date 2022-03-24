using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using ResultHandler.Utility;
using ResultHandler.Exceptions;

namespace ResultHandler;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger,
        RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)GetHttpStatusCodeByExceptionType(ex);
            await context.Response.WriteAsJsonAsync(ex.ToResult());

            _logger.LogError("::::::::::::::::::: Exception :::::::::::::::::::");
            _logger.LogError($"Message ::::::::::::::::::: {ex.Message} :::::::::::::::::::");
            _logger.LogError($"Inner Exception ::::::::::::::::::: {ex.GetException()} :::::::::::::::::::");
        }
    }

    private static HttpStatusCode GetHttpStatusCodeByExceptionType(Exception exception)
    {
        return exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            BadRequestException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            ApplicationException => HttpStatusCode.NotImplemented,
            _ => HttpStatusCode.InternalServerError,
        };
    }
}
