using System.Net;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Projects;

namespace Tests.IntegrationTests.EndPoints.Projects;

public class ProjectDeleteEndPointTests(WebApiFactory factory):IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task DeleteProject_ShouldDeleteProject_WhenExists()
    {
        // Arrange
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
        
        var testProject = await TestDbHelper.GetEntityAsync<Project>(
            factory.Services,
            u => u.Name == "Task API"
        );
        
        testProject.Should().NotBeNull(); // Ensure Project IS FOUND
        
        var response = await _client.DeleteAsync($"/api/projects/{testProject.Id}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task DeleteProject_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        var projectId=Guid.NewGuid();
        
        var response = await _client.DeleteAsync($"/api/projects/{projectId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}