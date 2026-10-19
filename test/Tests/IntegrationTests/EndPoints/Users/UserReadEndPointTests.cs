using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Projects;
using WebApi.Domain.Users;

namespace Tests.IntegrationTests.EndPoints.Users;

public class UserReadEndPointTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task GetUsers_ReturnsUsers_WhenUsersExist()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        
        // Act
        var response = await authClient.GetAsync("/api/users");
        var users = await response.Content.ReadFromJsonAsync<List<User>>(Helpers.GetJsonOption());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        users.Should().NotBeNull();
        users.Should().HaveCount(4);
    }
    
    [Fact]
    public async Task GetUsers_ReturnsEmptyList_WhenUsersDoseNotExist()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        await factory.ClearUsersTableAsync();
    
        // Act
        var response = await authClient.GetAsync("/api/users");
        var users = await response.Content.ReadFromJsonAsync<List<User>>(Helpers.GetJsonOption());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        users.Should().NotBeNull();
        users.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetUserById_ReturnsUser_WhenExists()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);

        var testUser = await GetUser(UserRole.User, "Alice Johnson");
        testUser.Should().NotBeNull(); // Ensure USER IS FOUND
        
        // Act
        var response = await authClient.GetAsync($"/api/users/{testUser.Id}");
        var foundUser = await response.Content.ReadFromJsonAsync<User>(Helpers.GetJsonOption());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        foundUser.Should().NotBeNull();
        foundUser.Id.Should().Be(testUser.Id);
        foundUser.Name.Should().Be(testUser.Name);
    }
    
    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenNotExists()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        var userId = Guid.NewGuid();
        
        // Act
        var response = await authClient.GetAsync($"/api/users/{userId}");
        
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