using System.Net;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Projects;
using WebApi.Domain.Users;

namespace Tests.IntegrationTests.EndPoints.Projects;

public class ProjectDeleteEndPointTests(WebApiFactory factory):IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task DeleteProject_ShouldDeleteProject_WhenExists()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        var testProject = await GetProject("Task API");
        testProject.Should().NotBeNull(); // Ensure Project IS FOUND
        
        var response = await authClient.DeleteAsync($"/api/projects/{testProject.Id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task DeleteProject_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        var projectId=Guid.NewGuid();
        
        var response = await authClient.DeleteAsync($"/api/projects/{projectId}");
        
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