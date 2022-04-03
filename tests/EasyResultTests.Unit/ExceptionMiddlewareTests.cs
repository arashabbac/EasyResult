using EasyResult;
using EasyResult.Services;
using EasyResultTests.Unit.Server;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace EasyResultTests.Unit;

public class ExceptionMiddlewareTests : TestFixture
{
    [Fact]
    public async Task BadRequest_Exception_Test()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/RaiseException");

        var result = await response.Content.ReadFromJsonAsync<Result>();

        result!.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain("This is bad request exception!");
        result.Successes.Should().BeEmpty();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task NotFound_Exception_Test()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/RaiseException/1");

        var result = await response.Content.ReadFromJsonAsync<Result>();

        result!.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain("This is not found exception!");
        result.Successes.Should().BeEmpty();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Not_Registered_BuiltIn_TaskCanceled_Exception_Test_Must_Return_Code_500()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/RaiseException/2");

        var result = await response.Content.ReadFromJsonAsync<Result>();

        result!.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain("This is task cancelled exception!(built-in exception that we do not register it!)");
        result.Successes.Should().BeEmpty();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Registered_BuiltIn_KeyNotFound_Exception_Test_Must_Return_Code_500()
    {
        var exService = Server.Services.GetRequiredService<ExceptionService>();
        exService.Add(typeof(KeyNotFoundException),HttpStatusCode.NotImplemented);

        var response = await Server.Host.GetTestClient().GetAsync("Fake/RaiseException/3");

        var result = await response.Content.ReadFromJsonAsync<Result>();

        result!.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain("This is key not found exception!(built-in exception that we register it!)");
        result.Successes.Should().BeEmpty();
        response.StatusCode.Should().Be(HttpStatusCode.NotImplemented);
    }

    [Fact]
    public async Task Unhandled_Exception_Test_Must_Return_Code_500()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/RaiseException/10");

        var result = await response.Content.ReadFromJsonAsync<Result>();

        result!.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain("This is an unhandled exception!");
        result.Successes.Should().BeEmpty();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}
