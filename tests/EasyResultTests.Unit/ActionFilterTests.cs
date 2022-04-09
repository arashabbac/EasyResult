using EasyResult.Services;
using EasyResultTests.Unit.Doubles;
using EasyResultTests.Unit.Doubles.FakeObjects;
using EasyResultTests.Unit.Server;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace EasyResultTests.Unit;

public class ActionFilterTests : TestFixture
{

    [Fact]
    public async Task BadRequest_Method_Test()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/Get");

        var result = await response.Content.ReadFromJsonAsync<FakeResult>();

        result!.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(JsonSerializer.Serialize("Id is required!"));
        result.Successes.Should().BeEmpty();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task BadRequest_Method_Test_Without_Object()
    {
        var response = await Server.Host.GetTestClient().PatchAsync("Fake/ChangeActivity", null);

        var result = await response.Content.ReadFromJsonAsync<FakeResult>();

        result!.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Successes.Should().BeEmpty();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task NotFound_Method_Test()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/Get/3");

        var result = await response.Content.ReadFromJsonAsync<FakeResult>();

        result!.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(JsonSerializer.Serialize("Person is not found!"));
        result.Successes.Should().BeEmpty();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task NotFound_Method_Test_Without_Object()
    {
        var response = await Server.Host.GetTestClient().PatchAsync("Fake/ChangeActivity/3", null);

        var result = await response.Content.ReadFromJsonAsync<FakeResult>();

        result!.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Successes.Should().BeEmpty();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ModelState_Failed_Test()
    {
        var response = await Server.Host.GetTestClient().PostAsJsonAsync("Fake/Post", new { });

        var result = await response.Content.ReadFromJsonAsync<FakeResult>();

        result!.IsSuccess.Should().BeFalse();
        result.Successes.Should().BeEmpty();
        result.Errors.Should().HaveCountGreaterThan(1);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Ok_Method_With_Object()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/Get/1");

        var result = await response.Content.ReadFromJsonAsync<FakeResult<Person>>();

        result!.Errors.Should().BeEmpty();
        result.IsSuccess.Should().BeTrue();
        result.Successes.Should().BeEmpty();
        result.Data.Should().BeEquivalentTo(new Person
        {
            Id = 1,
            FirstName = "Arash",
            LastName = "Abbac",
            IsActive = true,
        });
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Ok_Method_Without_Object()
    {
        var response = await Server.Host.GetTestClient().PatchAsync("Fake/ChangeActivity/2", null);

        var result = await response.Content.ReadFromJsonAsync<FakeResult>();

        result!.Errors.Should().BeEmpty();
        result.IsSuccess.Should().BeTrue();
        result.Successes.Should().HaveCount(1);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Retrun_BadRequest_With_Result_Object()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/Result");

        var result = await response.Content.ReadFromJsonAsync<FakeResult>();

        result!.Successes.Should().BeEmpty();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain("personId must have value!");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Retrun_Ok_With_Result_Object()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/Result/1");

        var result = await response.Content.ReadFromJsonAsync<FakeResult<Person>>();

        result!.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Successes.Should().BeEmpty();
        result.Data.Should().BeEquivalentTo(new Person
        {
            Id = 1,
            FirstName = "Arash",
            LastName = "Abbac",
            IsActive = true,
        }); 
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Return_FailedDependency_If_Success_Result_Comes_Without_200Range_Status_Code()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/Result/10");

        var result = await response.Content.ReadFromJsonAsync<FakeResult>();

        result!.Successes.Should().BeEmpty();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain("Incorrect Result object!," +
                            " You can not return successful result without 200 range status codes!");
        response.StatusCode.Should().Be(HttpStatusCode.FailedDependency);
    }

    [Fact]
    public async Task Return_FailedDependency_If_Error_Result_Comes_With_200Range_Status_Code()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/ErrorResult");

        var result = await response.Content.ReadFromJsonAsync<FakeResult>();

        result!.Successes.Should().BeEmpty();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain("Incorrect Result object!," +
                            " You can not return error result with 200 range status codes!");
        response.StatusCode.Should().Be(HttpStatusCode.FailedDependency);
    }

    [Fact]
    public async Task Return_BadRequest_If_Ok_Object_Data_Is_Null()
    {
        var exService = Server.Services.GetRequiredService<ExceptionService>();
        exService.Add(typeof(ArgumentNullException), HttpStatusCode.BadRequest);

        var response = await Server.Host.GetTestClient().GetAsync("Fake/NullData");
        
        var result = await response.Content.ReadFromJsonAsync<FakeResult>();

        result!.Successes.Should().BeEmpty();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain("Value cannot be null. (Parameter 'data')");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
