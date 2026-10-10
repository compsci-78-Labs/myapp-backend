using WebApi.Domain.Projects;
using WebApi.Features.Projects;

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
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
        
        // Act
        var response = await _client.GetAsync("/api/projects");
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
        await factory.ClearDatabaseTablesAsync();
    
        // Act
        var response = await _client.GetAsync("/api/projects");
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
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
        
        var testProject = await TestDbHelper.GetEntityAsync<Project>(
            factory.Services,
            u => u.Name == "Task API"
            );
        testProject.Should().NotBeNull(); // Ensure PROJECT IS FOUND
        
        // Act
        var response = await _client.GetAsync($"/api/projects/{testProject.Id}");
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
        var projectId = Guid.NewGuid();
        
        // Act
        var response = await _client.GetAsync($"/api/projects/{projectId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}