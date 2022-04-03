using EasyResult.Configurations;
using System;

namespace EasyResultTests.Unit.Doubles.Exceptions;

public class NotFoundException : Exception, IExceptionResult<NotFoundException>
{
    public NotFoundException()
    { }

    public NotFoundException(string message)
        : base(message)
    { }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    { }

    public void Configure(ExceptionResultBuilder<NotFoundException> exceptionBuilder)
    {
        exceptionBuilder.WithHttpStatusCode(System.Net.HttpStatusCode.NotFound);
    }
}
