using Microsoft.Extensions.DependencyInjection;
using WebApi.Domain.Projects;
using WebApi.Application.Projects;
using WebApi.Domain.Users;
using WebApi.Presentation.Projects;

namespace Tests.IntegrationTests.EndPoints.Projects;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;

public class ProjectReadEndPointTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task GetProjects_ReturnsProjects_WhenProjectsExist()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
            
        // Act
        var response = await authClient.GetAsync("/api/projects");
        var projects = await response.Content.ReadFromJsonAsync<List<ReadProjectResponse>>(Helpers.GetJsonOption());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        projects.Should().NotBeNull();
        projects.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task GetProjects_ReturnsEmptyList_WhenProjectsDoseNotExist()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        await factory.ClearProjectsTableAsync();
        
        // Act
        var response = await authClient.GetAsync("/api/projects");
        var projects = await response.Content.ReadFromJsonAsync<List<ReadProjectResponse>>(Helpers.GetJsonOption());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        projects.Should().NotBeNull();
        projects.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetProjectById_ReturnsProject_WhenExists()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);

        var testProject = await GetProject("Task API");
        testProject.Should().NotBeNull(); // Ensure PROJECT IS FOUND
        
        // Act
        var response = await authClient.GetAsync($"/api/projects/{testProject.Id}");
        var foundProject = await response.Content.ReadFromJsonAsync<ReadProjectResponse>(Helpers.GetJsonOption());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        foundProject.Should().NotBeNull();
        foundProject.Id.Should().Be(testProject.Id);
        foundProject.Name.Should().Be(testProject.Name);
        foundProject.Description.Should().Be(testProject.Description);
        foundProject.Owner.Should().Be(testProject.OwnerId);
    }
    
    [Fact]
    public async Task GetProjectById_ReturnsNotFound_WhenNotExists()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        var projectId = Guid.NewGuid();
        
        // Act
        var response = await authClient.GetAsync($"/api/projects/{projectId}");
        
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