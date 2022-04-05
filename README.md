# EasyResult

**EasyResult is a lightweight library that could be used in your API projects and can handle exceptions<br/>
You do not have to pass Result object throughout your project, This library is a wrapper on your endpoints
and convert your object into Result object for both success and error scenarios.**

You can install [EasyResult with NuGet](https://www.nuget.org/packages/EasyResult/)
```
Install-Package EasyResult
```

## Description
This library gives you two options for handling error scenarios:

    1. Returning ActionResult methods
    2. Throwing Exceptions

Every exception type has only one HttpStatusCode and you have to register it in your code so everytime that exception throws, API knows which HttpStatusCode should return

## Instalation

You need to register easy result service as below in ConfigureService method:

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers().AddEasyResult();
}
```

for handling exceptions you have to use exception middleware in Configure method:

```
public void Configure(IApplicationBuilder app)
{
    app.UseExceptionMiddleware();
}
```

## Configuration

There are two ways to register exceptions in EasyResult:
  1. Use IExceptionResult interface (this way can only be used for your customize exceptions).<br/>
    You have to inherit your customize exception from this generic interface and implement its method and assign your desireable HttpStatusCode
  ```
  public class BadRequestException : Exception, IExceptionResult<BadRequestException>
  {
      public BadRequestException()
      { }

      public BadRequestException(string message)
          : base(message)
      { }

      public BadRequestException(string message, Exception innerException)
          : base(message, innerException)
      { }

      public void Configure(ExceptionResultBuilder<BadRequestException> builder)
      {
          builder.WithHttpStatusCode(HttpStatusCode.BadRequest);
      }
  }
  ```
  
  2. Use ExceptionService<br/>
     In order to use this way, you have to write a middleware as code below:
  
  ```
  public static void AddExceptions(IApplicationBuilder app)
  {
      using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();
      var exceptionService = scope.ServiceProvider.GetRequiredService<ExceptionService>();
      
      exceptionService.Add(typeof(ApplicationException), HttpStatusCode.Conflict);
      exceptionService.Add(typeof(UnauthorizedAccessException),HttpStatusCode.Unauthorized);
  }
  ```
  If some exception throws and you did not assign its HttpStatusCode, then EasyResult marked it as an unhandled exception and return 500 HttpStatusCode.
  
