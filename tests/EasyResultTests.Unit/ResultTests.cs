using EasyResult;
using EasyResult.Configurations;
using EasyResultTests.Unit.Server;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Xunit;

namespace EasyResultTests.Unit;

public class ResultTests : TestFixture
{
    private readonly IOptions<ResultOptions> _options;
    public ResultTests()
    {
        _options = Server.Services.GetRequiredService<IOptions<ResultOptions>>();
    }

    [Fact]
    public void Result_With_Error()
    {
        var result = new Result(_options);

        result.WithError("This is error 1!");

        result.Errors.Should().HaveCount(1);
        result.IsSuccess.Should().BeFalse();
        result.Successes.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Multiple_Error()
    {
        var result = new Result(_options);

        result.WithError("This is error 1!");
        result.WithError("This is error 2!");

        result.Errors.Should().HaveCount(2);
        result.IsSuccess.Should().BeFalse();
        result.Successes.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Multiple_And_Duplicate_Error()
    {
        var result = new Result(_options);

        result.WithError("This is error 1!");
        result.WithError("This is error 1!");
        result.WithError("This is error 2!");

        result.Errors.Should().HaveCount(2);
        result.IsSuccess.Should().BeFalse();
        result.Successes.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("    ")]
    [InlineData("")]
    public void Result_With_Error_And_Incorrect_Messsages(string? message)
    {
        var result = new Result(_options);

        result.WithError(message!);

        result.Errors.Should().BeEmpty();
        result.IsSuccess.Should().BeFalse();
        result.Successes.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Success()
    {
        var result = new Result(_options);

        result.WithSuccess("This is success 1!");

        result.Successes.Should().HaveCount(1);
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Multiple_Success()
    {
        var result = new Result(_options);

        result.WithSuccess("This is success 1!");
        result.WithSuccess("This is success 2!");

        result.Successes.Should().HaveCount(2);
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Multiple_And_Duplicate_Success()
    {
        var result = new Result(_options);

        result.WithSuccess("This is success 1!");
        result.WithSuccess("This is success 1!");
        result.WithSuccess("This is success 2!");

        result.Successes.Should().HaveCount(2);
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("    ")]
    [InlineData("")]
    public void Result_With_Success_And_Incorrect_Messsages_Must_Return_Default_Message(string? message)
    {
        var result = new Result(_options);

        result.WithSuccess(message!);

        result.Successes.Should().HaveCount(1);
        result.Successes.Should().Contain(_options.Value.SuccessDefaultMessage);
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("    ")]
    [InlineData("")]
    public void Result_With_Success_And_Incorrect_Messsages_Must_Return_MyDefault_Message(string? message)
    {
        string myDefaultMessage = "عملیات با موفقیت انجام شد";
        _options.Value.SuccessDefaultMessage = myDefaultMessage;

        var result = new Result(_options);

        result.WithSuccess(message!);

        result.Successes.Should().HaveCount(1);
        result.Successes.Should().Contain(_options.Value.SuccessDefaultMessage);
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Success_And_Default_Message()
    {
        var result = new Result(_options);

        result.WithSuccess();

        result.Successes.Should().HaveCount(1);
        result.Successes.Should().Contain("Operation has been done successfully!");
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Data()
    {
        var result = new Result<object>(_options);

        var data = new { FirstName = "Arash", LastName = "Abbac" };

        result.WithData(data);

        result.Successes.Should().BeEmpty();
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Data.Should().BeEquivalentTo(data);
    }

    [Fact]
    public void Result_With_Null_Data()
    {
        var result = new Result<object>(_options);

        Action act = () => result.WithData(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Result_With_Data_And_Message()
    {
        var result = new Result<object>(_options);

        var data = new { FirstName = "Arash", LastName = "Abbac" };

        var message = "Operation successfull";
        result.WithSuccess(message).WithData(data);

        result.Successes.Should().HaveCount(1);
        result.Successes.Should().Contain(message);
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Data.Should().BeEquivalentTo(data);
    }
}
