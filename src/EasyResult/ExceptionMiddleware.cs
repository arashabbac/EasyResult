using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using EasyResult.Utility;
using EasyResult.Services;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

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
            var jsonOptions = context.RequestServices.GetService<IOptions<JsonSerializerOptions>>();
            context.Response.StatusCode = (int)_exceptionService.GetHttpStatusCodeByExceptionType(ex);

            if(jsonOptions != null)
                await context.Response.WriteAsJsonAsync(ex.ToResult(),jsonOptions.Value);
            else
                await context.Response.WriteAsJsonAsync(ex.ToResult());

            _logger.LogError("::::::::::::::::::: Exception :::::::::::::::::::");
            _logger.LogError($"Message ::::::::::::::::::: {ex.Message} :::::::::::::::::::");
            _logger.LogError($"Inner Exception ::::::::::::::::::: {ex.GetException()} :::::::::::::::::::");
        }
    }
}

