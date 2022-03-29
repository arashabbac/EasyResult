using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ResultHandler.Services;

namespace ResultHandler.Runtimes;

public static class ResultHandlerExtensions
{
    public static IMvcBuilder AddResultHandler(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.Services.AddSingleton<IExceptionService, ExceptionService>();
        mvcBuilder.AddMvcOptions(c => c.Filters.Add(typeof(ActionResultFilterAttribute)));
        return mvcBuilder;
    }

    public static IMvcCoreBuilder AddResultHandler(this IMvcCoreBuilder mvcBuilder)
    {
        mvcBuilder.Services.AddSingleton<IExceptionService, ExceptionService>();
        mvcBuilder.AddMvcOptions(c => c.Filters.Add(typeof(ActionResultFilterAttribute)));
        return mvcBuilder;
    }

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}
