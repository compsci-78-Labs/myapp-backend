using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Users;

namespace Tests.IntegrationTests.EndPoints.Users;

public class UserCreateEndPointsTests(WebApiFactory factory):IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateUser_ReturnsCreatedUser_WhenSuccess()
    {
        // Arrange
        var newUser = new User
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            PasswordHash = "password",
            CreatedAt = DateTime.UtcNow
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", newUser);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdUser = await response.Content.ReadFromJsonAsync<User>();
        
        // Assert
        createdUser.Should().NotBeNull();
        createdUser.Id.Should().NotBeEmpty();
        createdUser.Name.Should().Be("John Doe");
        createdUser.Email.Should().Be("john.doe@example.com");

    }
}