using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Users;

namespace Tests.IntegrationTests.EndPoints.Users;

public class UserDeleteEndPointTests(WebApiFactory factory):IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task DeleteUser_ShouldDeleteUser_WhenExists()
    {
        // Arrange
        var newUser = new User
        {
            Name = "To Be Deleted",
            PasswordHash = "Password",
            Email = "tobedeleted@example.com",
            CreatedAt = DateTime.UtcNow
        };
        var response = await _client.PostAsJsonAsync("/api/users", newUser);
        var createdUser = await response.Content.ReadFromJsonAsync<User>();
        
        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/users/{createdUser!.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // Assert
        var getResponse = await _client.GetAsync($"/api/users/{createdUser.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}