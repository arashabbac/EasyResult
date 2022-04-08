using EasyResult.Configurations;
using EasyResult.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace EasyResult;

public class ActionResultFilterAttribute : ActionFilterAttribute
{
    private readonly Result _result;
    private readonly IOptions<ResultOptions> _options;
    public ActionResultFilterAttribute(Result result, IOptions<ResultOptions> options)
    {
        _result = result;
        _options = options;
    }

    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objResult)
        {
            var value = ConvertObjectResult(objResult);

            var objectResult = new ObjectResult(value)
            {
                StatusCode = objResult.StatusCode,
                ContentTypes = objResult.ContentTypes,
                DeclaredType = objResult.DeclaredType
            };

            context.Result = objectResult;
        }
        if (context.Result is StatusCodeResult statusCodeResult)
        {

            var value = IsIn200Range(statusCodeResult.StatusCode) ?
                        _result.WithSuccess() :
                        _result.WithError();

            var objectResult = new ObjectResult(value)
            {
                StatusCode = statusCodeResult.StatusCode,
            };

            context.Result = objectResult;
        }
        if (context.ModelState.IsValid == false)
        {
            foreach (var item in context.ModelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    _result.WithError($"{item.Key}: {error.ErrorMessage}");
                }
            }
            context.Result = new BadRequestObjectResult(_result);
        }

        await base.OnResultExecutionAsync(context, next);
    }

    private static bool IsIn200Range(int? statusCode)
    {
        if (statusCode is null)
            throw new ApplicationException("Http Status code is null");

        return (HttpStatusCode)statusCode.Value switch
        {
            HttpStatusCode.OK => true,
            HttpStatusCode.Created => true,
            HttpStatusCode.NoContent => true,
            HttpStatusCode.PartialContent => true,
            HttpStatusCode.Accepted => true,
            HttpStatusCode.AlreadyReported => true,
            _ => false,
        };
    }

    private static bool IsResultType(object? obj) => obj is Result;

    private object? ConvertObjectResult(ObjectResult objectResult)
    {
        object? value = objectResult.Value;
        if (IsIn200Range(objectResult.StatusCode))
        {
            if (IsResultType(objectResult.Value) == false)
            {
                value = objectResult.Value.ToResult(_options);
            }
            else
            {
                var obj = objectResult.Value as Result;

                if (obj!.IsSuccess == false || obj!.Errors.Count > 0)
                    throw new ApplicationException("Incorrect Result object!," +
                        " You can not return error result with 200 range status codes!");
            }
        }
        else
        {
            if (IsResultType(objectResult.Value) == false)
            {
                value = _result.WithError(JsonSerializer.Serialize(objectResult.Value));
            }
            else
            {
                var obj = objectResult.Value as Result;

                if (obj!.IsSuccess || obj!.Successes.Count > 0)
                    throw new ApplicationException("Incorrect Result object!," +
                        " You can not return successful result without 200 range status codes!");
            }
        }

        return value;
    }
}
