using System.Net;
using FluentAssertions;
using Tests.Common;

namespace Tests.IntegrationTests.EndPoints;

public class UserEndPointsTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetUsers_ReturnsUsers_WhenUsersExist()
    {
        var response = await _client.GetAsync("/api/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}