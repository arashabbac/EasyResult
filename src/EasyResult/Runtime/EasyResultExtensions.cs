using EasyResult.Configurations;
using EasyResult.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ResultOptions = EasyResult.Configurations.ResultOptions;

namespace EasyResult.Runtime;

public static class EasyResultExtensions
{
    public static IMvcBuilder AddEasyResult(this IMvcBuilder mvcBuilder, Action<ResultOptions>? options = default)
    {
        mvcBuilder.Services.AddSingleton<ExceptionService>();
        mvcBuilder.Services.AddSingleton(typeof(ExceptionResultBuilder<>));
        mvcBuilder.Services.AddTransient<Result>();
        mvcBuilder.Services.AddTransient(typeof(Result<>));





        //  mvcBuilder.Services.Configure(options);
        ResultSetting.Setup(options);
        mvcBuilder.AddMvcOptions(c => c.Filters.Add(typeof(ActionResultFilterAttribute)));
        return mvcBuilder;
    }

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        AddExceptionsFromAssemblies(app);
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }

    private static void AddExceptionsFromAssemblies(IApplicationBuilder app, params Assembly[] assemblies)
    {
        var ass = AppDomain.CurrentDomain.GetAssemblies();
        ass.Concat(assemblies);
        var validExceptionTypes = ass
            .SelectMany(s => s.GetTypes())
            .Where(p => p.IsSubclassOf(typeof(Exception)) &&
                p.IsClass && !p.IsInterface && p.GetInterface(typeof(IExceptionResult<>).Name) is not null);


        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

        foreach (var ex in validExceptionTypes)
        {
            var method = ex.GetMethod("Configure");
            var wrapper = typeof(ExceptionResultBuilder<>).MakeGenericType(ex);
            var genericBuilder = scope.ServiceProvider.GetService(wrapper);

            var exceptionObj = Activator.CreateInstance(ex);

            method!.Invoke(exceptionObj, new object[] { genericBuilder! });
        }
    }
}
