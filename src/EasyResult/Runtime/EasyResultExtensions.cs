using EasyResult.Configurations;
using EasyResult.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EasyResult.Runtime;

public static class EasyResultExtensions
{
    public static IMvcBuilder AddEasyResult(this IMvcBuilder mvcBuilder)
    {
      mvcBuilder.Services.AddSingleton<ExceptionService>();
      mvcBuilder.Services.AddSingleton(typeof(ExceptionResultBuilder<>));


        mvcBuilder.AddMvcOptions(c => c.Filters.Add(typeof(ActionResultFilterAttribute)));
        return mvcBuilder;
    }

    //public static IMvcCoreBuilder AddEasyResult(this IMvcCoreBuilder mvcBuilder)
    //{
    //    mvcBuilder.Services.AddSingleton<ExceptionService>();
    //    mvcBuilder.AddMvcOptions(c => c.Filters.Add(typeof(ActionResultFilterAttribute)));

    //    return mvcBuilder;
    //}

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
            .Where(p => p.IsSubclassOf(typeof(Exception)) && p.IsClass &&!p.IsInterface && p.GetInterface(typeof(IExceptionResult<>).Name) is not null);// ???


        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();
        //var exceptionService = scope.ServiceProvider.GetRequiredService<ExceptionService>();
       

        foreach (var ex in validExceptionTypes)
        {
            var method = ex.GetMethod("Configure");
           var wrapper = typeof(ExceptionResultBuilder<>).MakeGenericType(ex);
           var genericBuilder = scope.ServiceProvider.GetService(wrapper);

            //  Type genericBuilderType = typeof(ExceptionResultBuilder<>).MakeGenericType(ex.UnderlyingSystemType);
            //  var genericBuilder = Activator.CreateInstance(genericBuilderType, new object[] { exceptionService });

            var exceptionObj = Activator.CreateInstance(ex);

            method!.Invoke(exceptionObj, new object[] { genericBuilder! });
        }
    }
}
