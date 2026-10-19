using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Users;
using WebApi.Application.Users;
using WebApi.Domain.Projects;
using WebApi.Presentation.Users;

namespace Tests.IntegrationTests.EndPoints.Users;

public class UserUpdateEndpointTests(WebApiFactory factory):IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task UpdateUser_ShouldReturnUpdatedUser_WhenSuccess()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        var testUser = await GetUser(UserRole.User,"Alice Johnson");
        
        testUser.Should().NotBeNull(); // Ensure USER IS FOUND
        
        var request = new UpdateUserRequest(
            "Updated Name",
            "updated@example.com",
            UserRole.Admin);
        
        // Act
        var response = await authClient.PutAsJsonAsync(
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
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        
        var userId=Guid.NewGuid();
        var request = new UpdateUserRequest(
            "Updated Name",
            "updated@example.com",
            UserRole.Admin);
        
        var response = await authClient.PutAsJsonAsync(
            $"/api/users/{userId}",
            request,
            Helpers.GetJsonOption());
        
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