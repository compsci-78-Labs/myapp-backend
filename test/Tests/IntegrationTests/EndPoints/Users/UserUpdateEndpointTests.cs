using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Users;
using WebApi.Features.Users;

namespace Tests.IntegrationTests.EndPoints.Users;

public class UserUpdateEndpointTests(WebApiFactory factory):IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task UpdateUser_ShouldReturnUpdatedUser_WhenSuccess()
    {
        // Arrange
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
        
        var testUser = await TestDbHelper.GetEntityAsync<User>(
            factory.Services,
            u => u.Name == "Alice Johnson"
        );
        testUser.Should().NotBeNull(); // Ensure USER IS FOUND
        
        var request = new UpdateUserRequest(
            "Updated Name",
            "updated@example.com",
            UserRole.Admin);
        
        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/users/{testUser.Id}", 
            request,
            Helpers.GetJsonOption());

        var updatedUser = await response.Content.ReadFromJsonAsync<ReadUserResponse>(Helpers.GetJsonOption());
        
        // Assert
        updatedUser.Should().NotBeNull();
        updatedUser.Name.Should().Be("Updated Name");
        updatedUser.Email.Should().Be("updated@example.com");
        updatedUser.Role.Should().Be(UserRole.Admin);
    }
    [Fact]
    public async Task UpdateUser_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        var userId=Guid.NewGuid();
        var request = new UpdateUserRequest(
            "Updated Name",
            "updated@example.com",
            UserRole.Admin);
        
        var response = await _client.PutAsJsonAsync(
            $"/api/users/{userId}",
            request,
            Helpers.GetJsonOption());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}