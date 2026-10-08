using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Tests.Common;
using WebApi.Domain.Users;
using WebApi.Features.Users;
using WebApi.Serialization;
using Xunit.Abstractions;

namespace Tests.IntegrationTests.EndPoints.Users;

public class UserCreateEndPointTests(
    WebApiFactory factory, 
    ITestOutputHelper output
    ) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    

    [Fact]
    public async Task CreateUser_ReturnsCreatedUser_WhenSuccess()
    {
        // Arrange
        var request = new CreateUserRrequest(
            "John Doe",
            "john.doe@example.com",
            UserRole.User
        );
        
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Converters =
            {
                new JsonStringEnumConverter(new LowerCaseNamingPolicy())
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/users",
            request,
            jsonOptions
        );

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadAsStringAsync();
        output.WriteLine($"RESPONSE: {responseBody}");

        var createdUser = JsonSerializer.Deserialize<ReadUserResponse>(
            responseBody,
            jsonOptions
        );
        
        // Assert
        createdUser.Should().NotBeNull();
        createdUser!.Id.Should().NotBeEmpty();
        createdUser.Name.Should().Be("John Doe");
        createdUser.Email.Should().Be("john.doe@example.com");
        createdUser.Role.Should().Be(UserRole.User);
    }
}