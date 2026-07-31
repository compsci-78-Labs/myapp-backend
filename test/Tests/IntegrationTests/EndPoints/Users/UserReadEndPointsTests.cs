using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Users;

namespace Tests.IntegrationTests.EndPoints.Users;

public class UserReadEndPointsTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task GetUsers_ReturnsUsers_WhenUsersExist()
    {
        // Arrange
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
        
        // Act
        var response = await _client.GetAsync("/api/users");
        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        users.Should().NotBeNull();
        users.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task GetUsers_ReturnsEmptyList_WhenUsersDoseNotExist()
    {
        // Arrange
        await factory.ClearDatabaseTablesAsync();
    
        // Act
        var response = await _client.GetAsync("/api/users");
        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        users.Should().NotBeNull();
        users.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetUserById_ReturnsUser_WhenExists()
    {
        // Arrange
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
        
        var testUser = await TestDbHelper.GetEntityAsync<User>(
            factory.Services,
            u => u.Name == "Alice Johnson"
            );
        testUser.Should().NotBeNull(); // Ensure USER IS FOUND
        
        // Act
        var response = await _client.GetAsync($"/api/users/{testUser.Id}");
        var foundUser = await response.Content.ReadFromJsonAsync<User>();
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        foundUser.Should().NotBeNull();
        foundUser.Id.Should().Be(testUser.Id);
        foundUser.Name.Should().Be(testUser.Name);
    }
}