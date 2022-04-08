using EasyResult.Runtime;
using EasyResult.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;

namespace EasyResultTests.Unit.Server;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddEasyResult();
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseExceptionMiddleware();
        app.AddExceptions();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}


public static class ApplicationBuilderException
{
    public static void AddExceptions(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();
        var exceptionService = scope.ServiceProvider.GetRequiredService<ExceptionService>();

        exceptionService.Add(typeof(ApplicationException), HttpStatusCode.FailedDependency);
        exceptionService.Add(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized);
    }
}