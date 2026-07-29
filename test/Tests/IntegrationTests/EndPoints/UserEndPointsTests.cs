using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using Tests.Common;
using WebApi.Domain.Users;
using WebApi.Features.Users;

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
        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        users.Should().NotBeNull();
        users.Should().HaveCount(2);
        users[0].Name.Should().Be("Alice Johnson");
        users[1].Name.Should().Be("Bob Smith");
    }
    
    [Fact]
    public async Task GetUsers_ReturnsEmptyList_WhenUsersDoseNotExist()
    {
        // Arrange
        //await factory.SeedDatabaseAsync();
        
        // Act
        var response = await _client.GetAsync("/api/users");
        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        users.Should().NotBeNull();
        users.Should().HaveCount(0);
    }
}