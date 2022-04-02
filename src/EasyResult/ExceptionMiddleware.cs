using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using EasyResult.Utility;
using EasyResult.Services;

namespace EasyResult;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly ExceptionService _exceptionService;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger,
        RequestDelegate next, ExceptionService exceptionService)
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

