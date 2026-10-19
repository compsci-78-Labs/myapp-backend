using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Projects;
using WebApi.Domain.Users;
using WebApi.Application.Projects;
using WebApi.Presentation.Projects;

namespace Tests.IntegrationTests.EndPoints.Projects;

public class ProjectUpdateEndPointTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task UpdateProject_ShouldReturnUpdatedProject_WhenSuccess()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        var testUser = await GetUser(UserRole.User, "Bob Smith");
        testUser.Should().NotBeNull(); // Ensure User IS FOUND
        var testProject = await GetProject("Task API");
        testProject.Should().NotBeNull(); // Ensure Project IS FOUND

        var request = new UpdateProjectRequest(
            "API Task",
            "Developing API for the task management app",
            testUser.Id);

        // Act
        var response = await authClient.PutAsJsonAsync(
            $"/api/projects/{testProject.Id}",
            request,
            Helpers.GetJsonOption());

        var updatedProject = await response.Content.ReadFromJsonAsync<ReadProjectResponse>(Helpers.GetJsonOption());

        // Assert
        updatedProject.Should().NotBeNull();
        updatedProject.Id.Should().Be(testProject.Id);
        updatedProject.Name.Should().Be("API Task");
        updatedProject.Description.Should().Be("Developing API for the task management app");
        updatedProject.Owner.Should().Be(testUser.Id);
    }

    [Fact]
    public async Task UpdateProject_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);

        var request = new UpdateProjectRequest(
            "Updated Name",
            "updated@example.com",
            Guid.NewGuid());

        var response = await authClient.PutAsJsonAsync(
            $"/api/projects/{projectId}",
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