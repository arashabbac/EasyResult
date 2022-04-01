using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ResultHandler.Exceptions;
using ResultHandler.Utility;
using System.Net;
using System.Text.Json;

namespace ResultHandler;

public class ActionResultFilterAttribute : ActionFilterAttribute
{
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if(context.Result is ObjectResult)
        {
            var result = (ObjectResult)context.Result;

            var value = IsIn200Range(result.StatusCode) ? 
                        result.Value.ToResult() :
                        new Result().WithError(JsonSerializer.Serialize(result.Value));

            var objectResult = new ObjectResult(value)
            {
                StatusCode = result.StatusCode,
                ContentTypes = result.ContentTypes,
                DeclaredType = result.DeclaredType
            };

            context.Result = objectResult;
        }
        if(context.Result is StatusCodeResult)
        {
            var result = (StatusCodeResult)context.Result;

            var value = IsIn200Range(result.StatusCode) ? 
                        new Result().WithSuccess() :
                        new Result().WithError();

            var objectResult = new ObjectResult(value)
            {
                StatusCode = result.StatusCode,
            };

            context.Result = objectResult;
        }
        if (context.ModelState.IsValid == false)
        {
            var result = new Result();
            foreach (var item in context.ModelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    result.WithError($"{item.Key}: {error.ErrorMessage}");
                }
            }
            context.Result = new BadRequestObjectResult(result);
        }

        await base.OnResultExecutionAsync(context, next);
    }

    private static bool IsIn200Range(int? statusCode)
    {
        if(statusCode is null) 
            throw new BadRequestException("Http Status code is null");

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
}
