using System.Net;
using System.Net.Http.Json;
using System.Security.Principal;
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
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
        
        var testUser = await TestDbHelper.GetEntityAsync<User>(
            factory.Services,
            u => u.Name == "Alice Johnson"
        );
        testUser.Should().NotBeNull(); // Ensure USER IS FOUND
        
        var response = await _client.DeleteAsync($"/api/users/{testUser.Id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task DeleteUser_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        var userId=Guid.NewGuid();
        
        var response = await _client.DeleteAsync($"/api/users/{userId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}