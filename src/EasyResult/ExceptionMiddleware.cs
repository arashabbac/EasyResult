using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using EasyResult.Utility;
using EasyResult.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

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
            var jsonOptions = context.RequestServices.GetService<IOptions<JsonOptions>>();
            context.Response.StatusCode = (int)_exceptionService.GetHttpStatusCodeByExceptionType(ex);

            if(jsonOptions is not null && jsonOptions?.Value is not null)
                await context.Response.WriteAsJsonAsync(ex.ToResult(),jsonOptions.Value.JsonSerializerOptions);
            else
                await context.Response.WriteAsJsonAsync(ex.ToResult());

            _logger.LogError("::::::::::::::::::: Exception :::::::::::::::::::");
            _logger.LogError($"Message ::::::::::::::::::: {ex.Message} :::::::::::::::::::");
            _logger.LogError($"Inner Exception ::::::::::::::::::: {ex.GetException()} :::::::::::::::::::");
        }
    }
}

