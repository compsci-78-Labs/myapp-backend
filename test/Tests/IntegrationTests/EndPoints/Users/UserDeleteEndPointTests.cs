using System.Net;
using System.Net.Http.Json;
using System.Security.Principal;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Projects;
using WebApi.Domain.Users;
using Xunit.Abstractions;

namespace Tests.IntegrationTests.EndPoints.Users;

public class UserDeleteEndPointTests(
    WebApiFactory factory,
    ITestOutputHelper output
    ):IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task DeleteUser_ShouldDeleteUser_WhenExists()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);

        var testUser = await GetUser(UserRole.User, "Alice Johnson");
        testUser.Should().NotBeNull(); // Ensure USER IS FOUND
                         
        var response = await authClient.DeleteAsync($"/api/users/{testUser.Id}");
        var responseBody = await response.Content.ReadAsStringAsync();

        output.WriteLine($"Status Code: {response.StatusCode}");
        output.WriteLine($"Response Body: {responseBody}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task DeleteUser_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        var userId=Guid.NewGuid();
        
        var response = await authClient.DeleteAsync($"/api/users/{userId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    private async Task<HttpClient> GetAuthenticatedHttpClient(UserRole user)
    {
        var authUser = await TestDbHelper.GetEntityAsync<User>(
            factory.Services,
            u => u.Role == user);

        return await factory.CreateAuthenticatedClientAsync(authUser!);
    }
    
    private async Task<Project?> GetProject(string name)
    {
        return await TestDbHelper.GetEntityAsync<Project>(
            factory.Services,
            p =>  p.Name == name);
    }
    private async Task<User?> GetUser(UserRole role, string name)
    {
        return await TestDbHelper.GetEntityAsync<User>(
            factory.Services,
            u => u.Role == role && u.Name == name);
    }
    private async Task SeedDatabase()
    {
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
    }
}