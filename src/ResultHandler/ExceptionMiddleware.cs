using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using ResultHandler.Utility;
using ResultHandler.Exceptions;
using ResultHandler.Services;
using Microsoft.AspNetCore.Builder;

namespace ResultHandler;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IExceptionService _exceptionService;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger,
        RequestDelegate next, IExceptionService exceptionService)
    {
        _logger = logger;
        _next = next;
        _exceptionService = exceptionService;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)_exceptionService.GetHttpStatusCodeByExceptionType(ex);
            await context.Response.WriteAsJsonAsync(ex.ToResult());

            _logger.LogError("::::::::::::::::::: Exception :::::::::::::::::::");
            _logger.LogError($"Message ::::::::::::::::::: {ex.Message} :::::::::::::::::::");
            _logger.LogError($"Inner Exception ::::::::::::::::::: {ex.GetException()} :::::::::::::::::::");
        }
    }
}

