using System.Net;

namespace EasyResult.Configurations;

public class ResultOptions
{
    public string SuccessDefaultMessage { get; set; } = "Operation has been done successfully!";
    public HttpStatusCode UnhandledExceptionStatusCode { get; set; } = HttpStatusCode.InternalServerError;
}
