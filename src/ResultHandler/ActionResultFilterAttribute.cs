using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ResultHandler.Utility;

namespace ResultHandler;

public class ActionResultFilterAttribute : ActionFilterAttribute
{
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if(context.Result is OkObjectResult { StatusCode: 200 })
        {
            var result = (OkObjectResult)context.Result;
            context.Result = new OkObjectResult(result.Value.ToResult());
        }
        if (context.Result is OkResult { StatusCode: 200 })
        {
            context.Result = new OkObjectResult(new Result().WithSuccess());
        }
        await base.OnResultExecutionAsync(context, next);
    }
}
