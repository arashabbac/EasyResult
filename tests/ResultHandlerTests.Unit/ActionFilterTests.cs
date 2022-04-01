using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using ResultHandlerTests.Unit.Doubles.FakeObjects;
using ResultHandlerTests.Unit.Server;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ResultHandlerTests.Unit;

public class ActionFilterTests : TestFixture
{

    [Fact]
    public async Task BadRequest_Method_Test()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/Get");

        var result = await response.Content.ReadFromJsonAsync<ResultHandler.Result>();

        result!.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(JsonSerializer.Serialize("Id is required!"));
        result.Successes.Should().BeEmpty();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task BadRequest_Method_Test_Without_Object()
    {
        var response = await Server.Host.GetTestClient().PatchAsync("Fake/ChangeActivity",null);

        var result = await response.Content.ReadFromJsonAsync<ResultHandler.Result>();

        result!.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Successes.Should().BeEmpty();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task NotFound_Method_Test()
    {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/Get/3");

        var result = await response.Content.ReadFromJsonAsync<ResultHandler.Result>();

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

        var result = await response.Content.ReadFromJsonAsync<ResultHandler.Result>();

        result!.IsSuccess.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Successes.Should().BeEmpty();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ModelState_Failed_Test()
    {
        var response = await Server.Host.GetTestClient().PostAsJsonAsync("Fake/Post",new {});

        var result = await response.Content.ReadFromJsonAsync<ResultHandler.Result>();

        result!.IsSuccess.Should().BeFalse();
        result.Successes.Should().BeEmpty();
        result.Errors.Should().HaveCountGreaterThan(1);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Ok_Method_With_Object()
     {
        var response = await Server.Host.GetTestClient().GetAsync("Fake/Get/1");

        var result = await response.Content.ReadFromJsonAsync<ResultHandler.Result<Person>>();

        result!.Errors.Should().BeEmpty();
        result.IsSuccess.Should().BeTrue();
        result.Successes.Should().HaveCount(1);
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
        var response = await Server.Host.GetTestClient().PatchAsync("Fake/ChangeActivity/2",null);

        var result = await response.Content.ReadFromJsonAsync<ResultHandler.Result>();

        result!.Errors.Should().BeEmpty();
        result.IsSuccess.Should().BeTrue();
        result.Successes.Should().HaveCount(1);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
