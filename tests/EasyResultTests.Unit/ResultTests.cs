using EasyResult;
using FluentAssertions;
using Xunit;

namespace EasyResultTests.Unit;

public class ResultTests
{
    [Fact]
    public void Result_With_Error()
    {
        var result = new Result();

        result.WithError("This is error 1!");

        result.Errors.Should().HaveCount(1);
        result.IsSuccess.Should().BeFalse();
        result.Successes.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Multiple_Error()
    {
        var result = new Result();

        result.WithError("This is error 1!");
        result.WithError("This is error 2!");

        result.Errors.Should().HaveCount(2);
        result.IsSuccess.Should().BeFalse();
        result.Successes.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Multiple_And_Duplicate_Error()
    {
        var result = new Result();

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
        var result = new Result();

        result.WithError(message!);

        result.Errors.Should().BeEmpty();
        result.IsSuccess.Should().BeFalse();
        result.Successes.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Success()
    {
        var result = new Result();

        result.WithSuccess("This is success 1!");

        result.Successes.Should().HaveCount(1);
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Multiple_Success()
    {
        var result = new Result();

        result.WithSuccess("This is success 1!");
        result.WithSuccess("This is success 2!");

        result.Successes.Should().HaveCount(2);
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Multiple_And_Duplicate_Success()
    {
        var result = new Result();

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
    public void Result_With_Success_And_Incorrect_Messsages(string? message)
    {
        var result = new Result();

        result.WithSuccess(message!);

        result.Successes.Should().BeEmpty();
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Success_And_Default_Message()
    {
        var result = new Result();

        result.WithSuccess();

        result.Successes.Should().HaveCount(1);
        result.Successes.Should().Contain("Operation has been done successfully!");
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Result_With_Data()
    {
        var result = new Result<object>();

        var data = new { FirstName = "Arash", LastName = "Abbac" };

        result.WithData(data);

        result.Successes.Should().HaveCount(1);
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Data.Should().BeEquivalentTo(data);
    }

    [Fact]
    public void Result_With_Data_And_Message()
    {
        var result = new Result<object>();

        var data = new { FirstName = "Arash", LastName = "Abbac" };

        var message = "Operation successfull";
        result.WithData(data, message);

        result.Successes.Should().HaveCount(1);
        result.Successes.Should().Contain(message);
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Data.Should().BeEquivalentTo(data);
    }
}
