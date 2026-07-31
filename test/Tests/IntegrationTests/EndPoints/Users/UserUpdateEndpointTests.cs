using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Users;

namespace Tests.IntegrationTests.EndPoints.Users;

public class UserUpdateEndpointTests(WebApiFactory factory):IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task UpdateUser_ShouldReturnUpdatedUser_WhenSuccess()
    {
        // Arrange
        var newUser = new User
        {
            Name = "Original Name",
            PasswordHash = "password",
            Email = "original.email@example.com",
            CreatedAt =  DateTime.UtcNow
        };
        
        var response = await _client.PostAsJsonAsync("/api/users", newUser);
        var createdUser = await response.Content.ReadFromJsonAsync<User>();

        var updatedUser = new User
        {
            Id = createdUser.Id, 
            Name = "Updated Name",
            Email = "updated.email@example.com",
            PasswordHash = "newPassword",
            CreatedAt = createdUser.CreatedAt
        };
        
        // Act
        var updateResponse = await _client.PutAsJsonAsync($"/api/users/{createdUser.Id}", updatedUser);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await updateResponse.Content.ReadFromJsonAsync<User>();


    }
}