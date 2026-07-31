using System.Net;
using FluentAssertions;
using Tests.Common;

namespace Tests.IntegrationTests.EndPoints;

public class TaskItemEndPointsTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetTaskItems_ReturnsUsers()
    {
        var response = await _client.GetAsync("/api/taskitems");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}