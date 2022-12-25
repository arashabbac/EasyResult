using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using EasyResult.Utility;
using EasyResult.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

namespace EasyResult;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly ExceptionService _exceptionService;
    private readonly JsonOptions _jsonOptions;
    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger,
        RequestDelegate next, ExceptionService exceptionService,
        IOptions<JsonOptions> jsonOptions)
    {
        _logger = logger;
        _next = next;
        _exceptionService = exceptionService;
        _jsonOptions = jsonOptions.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var statusCode = (int)_exceptionService.GetHttpStatusCodeByException(ex);
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(ex.ToResult(),_jsonOptions.JsonSerializerOptions);

            if(statusCode == 500)
            {
                _logger.LogError("::::::::::::::::::: Exception - InternalServerError - 500 :::::::::::::::::::");
                _logger.LogError($"Message ::::::::::::::::::: {ex.Message} :::::::::::::::::::");
                _logger.LogError($"Inner Exception ::::::::::::::::::: {ex.GetException()} :::::::::::::::::::");
            }
        }
    }
}

