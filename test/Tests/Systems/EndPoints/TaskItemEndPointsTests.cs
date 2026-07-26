using System.Net;
using FluentAssertions;
using Tests.Common;

namespace Tests.Systems.EndPoints;

public class TaskItemEndPointsTests:IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;

    public TaskItemEndPointsTests(WebApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTaskItems_ReturnsUsers()
    {
        var response = await _client.GetAsync("/api/taskitems");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}