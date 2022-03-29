using FluentAssertions;
using ResultHandler.Exceptions;
using ResultHandler.Services;
using System;
using System.Net;
using Xunit;

namespace ResultHandlerTests.Unit;

public class ExceptionServiceTests
{
    private readonly IExceptionService _exceptionService;

    public ExceptionServiceTests()
    {
        _exceptionService = new ExceptionService();
    }

    [Fact]
    public void Check_BuiltIn_Exceptions()
    {
        var exceptions = _exceptionService.GetExceptions();

        exceptions.Should().HaveCount(3);
    }

    [Fact]
    public void Throw_Exception_On_Add_If_Type_Is_Not_Exception()
    {
        var ex = new ExceptionDto(typeof(CollectionAttribute), HttpStatusCode.BadRequest);

        Action act = () => _exceptionService.AddException(ex);
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("Only exception type is allowed!");
    }

    [Fact]
    public void Throw_Exception_On_Add_If_Exception_Is_Duplicated()
    {
        var ex = new ExceptionDto(typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized);

        Action act = () => _exceptionService.AddException(ex);

        act.Should().Throw<DuplicateWaitObjectException>()
            .Where(c=> c.Message.Contains("This exception type has already defined!"));
    }

    [Fact]
    public void Throw_Exception_On_Add_Null_Object()
    {
        Action act = () => _exceptionService.AddException(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Add_Exception_Successfully()
    {
        var ex = new ExceptionDto(typeof(ApplicationException), HttpStatusCode.NotImplemented);

        _exceptionService.AddException(ex);

        var exceptions = _exceptionService.GetExceptions();
        exceptions.Should().Contain(ex);
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
}
