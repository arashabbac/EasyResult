using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ResultHandler.Configurations;
using ResultHandler.Exceptions;
using ResultHandler.Services;
using System.Reflection;

namespace ResultHandler.Runtimes;

public static class ResultHandlerExtensions
{
    public static IMvcBuilder AddResultHandler(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.Services.AddSingleton<ExceptionService>();
        mvcBuilder.AddMvcOptions(c => c.Filters.Add(typeof(ActionResultFilterAttribute)));
        return mvcBuilder;
    }

    public static IMvcCoreBuilder AddResultHandler(this IMvcCoreBuilder mvcBuilder)
    {
        mvcBuilder.Services.AddSingleton<ExceptionService>();
        mvcBuilder.AddMvcOptions(c => c.Filters.Add(typeof(ActionResultFilterAttribute)));
        return mvcBuilder;
    }

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        AddExceptionsFromAssemblies(app);
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }

    private static void AddExceptionsFromAssemblies(IApplicationBuilder app)
    {
        var allExceptionTypesInAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.IsSubclassOf(typeof(Exception)) && p.IsClass);

        var validExceptionTypes = new List<Type>();

        foreach (Type type in allExceptionTypesInAssemblies)
        {
            if (type.GetInterface(typeof(IExceptionResult<>).Name.ToString()) is not null)
            {
                validExceptionTypes.Add(type);
            }
        }

        foreach (var ex in validExceptionTypes)
        {
            var method = ex.GetMethod("Configure");

            Type genericBuilder = typeof(ExceptionResultBuilder<>).MakeGenericType(ex.UnderlyingSystemType);

            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();
            var exceptionService = scope.ServiceProvider.GetRequiredService<ExceptionService>();

            var builderObj = Activator.CreateInstance(genericBuilder,new object[] {exceptionService});
            var exceptionObj = Activator.CreateInstance(ex);

            method!.Invoke(exceptionObj, new object[] { builderObj! });
        }
    }
}
