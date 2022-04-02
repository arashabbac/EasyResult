using EasyResult.Exceptions;
using EasyResult.Services;
using EasyResultTests.Unit.Server;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using Xunit;

namespace EasyResultTests.Unit;

public class ExceptionServiceTests : TestFixture
{
    private readonly ExceptionService _exceptionService;

    public ExceptionServiceTests()
    {
        _exceptionService = Server.Services.GetRequiredService<ExceptionService>();
    }

    [Fact]
    public void Throw_Exception_On_Add_If_Type_Is_Not_Exception()
    {
        Action act = () => _exceptionService.Add(typeof(CollectionAttribute), HttpStatusCode.BadRequest);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Only exception type is allowed!");
    }

    [Fact]
    public void Throw_Exception_On_Add_If_Exception_Is_Duplicated()
    {
        _exceptionService.Add(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized);

        Action act = () => _exceptionService.Add(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized);

        act.Should().Throw<DuplicateWaitObjectException>()
            .Where(c => c.Message.Contains("This exception type has already defined!"));
    }

    [Fact]
    public void Throw_Exception_On_Add_Null_Object()
    {
        Action act = () => _exceptionService.Add(null!, HttpStatusCode.Forbidden);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Add_Exception_Successfully()
    {
        _exceptionService.Add(typeof(ApplicationException), HttpStatusCode.NotImplemented);

        var exceptions = _exceptionService.GetExceptions();
        exceptions.Keys.Should().Contain(typeof(ApplicationException));
        exceptions.Values.Should().Contain(HttpStatusCode.NotImplemented);
    }

    [Fact]
    public void Get_InternalServerError_StatusCode_If_Exception_Is_Not_Defined()
    {
        var statusCode = _exceptionService.GetHttpStatusCodeByExceptionType(new AggregateException());

        statusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public void Get_NotFound_StatusCode_For_NotFoundException()
    {
        var statusCode = _exceptionService.GetHttpStatusCodeByExceptionType(new NotFoundException());

        statusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public void Get_BadRequest_StatusCode_For_BadRequestException()
    {
        var statusCode = _exceptionService.GetHttpStatusCodeByExceptionType(new BadRequestException());

        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
