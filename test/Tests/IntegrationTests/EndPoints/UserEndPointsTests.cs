using System.Net;
using FluentAssertions;
using Tests.Common;

namespace Tests.IntegrationTests.EndPoints;

public class UserEndPointsTests:IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;

    public UserEndPointsTests(WebApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUsers_ReturnsUsers()
    {
        var response = await _client.GetAsync("/api/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}