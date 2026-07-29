using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Users;

namespace Tests.IntegrationTests.EndPoints;

public class UserEndPointsTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetUsers_ReturnsUsers_WhenUsersExist()
    {
        // Arrange
        await factory.SeedDatabaseAsync();
        
        // Act
        var response = await _client.GetAsync("/api/users");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        users.Should().NotBeNullOrEmpty();
        users.Count.Should().Be(2);

    }
}