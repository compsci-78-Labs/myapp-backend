using System.Net;
using FluentAssertions;
using Tests.Common;

namespace Tests.IntegrationTests.EndPoints;

public class ProjectEndPointsTests:IClassFixture<WebApiFactory>
{
private readonly HttpClient _client;

public ProjectEndPointsTests(WebApiFactory factory)
{
    _client = factory.CreateClient();
}

[Fact]
public async Task GetUsers_ReturnsUsers()
{
    var response = await _client.GetAsync("/api/projects");
    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
}