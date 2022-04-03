using EasyResult.Configurations;
using System;

namespace EasyResultTests.Unit.Doubles.Exceptions;

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
        builder.WithHttpStatusCode(System.Net.HttpStatusCode.BadRequest);
    }
}