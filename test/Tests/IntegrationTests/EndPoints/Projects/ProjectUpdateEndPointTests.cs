using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Projects;
using WebApi.Domain.Users;
using WebApi.Features.Projects;

namespace Tests.IntegrationTests.EndPoints.Projects;

public class ProjectUpdateEndPointTests(WebApiFactory factory):IClassFixture<WebApiFactory>
{
private readonly HttpClient _client = factory.CreateClient();

[Fact]
public async Task UpdateProject_ShouldReturnUpdatedProject_WhenSuccess()
{
    // Arrange
    await factory.ClearDatabaseTablesAsync();
    await factory.SeedDatabaseAsync();

    var testUser= await TestDbHelper.GetEntityAsync<User>(
        factory.Services,
        u => u.Name == "Bob Smith"
    );
    testUser.Should().NotBeNull(); // Ensure User IS FOUND
    
    var testProject = await TestDbHelper.GetEntityAsync<Project>(
        factory.Services,
        u => u.Name == "Task API"
    );
    testProject.Should().NotBeNull(); // Ensure Project IS FOUND

    var request = new UpdateProjectRequest(
        "API Task",
        "Developing API for the task management app",
        testUser.Id);

    // Act
    var response = await _client.PutAsJsonAsync(
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
    
    var request = new UpdateProjectRequest(
        "Updated Name",
        "updated@example.com",
        Guid.NewGuid());

    var response = await _client.PutAsJsonAsync(
        $"/api/projects/{projectId}",
        request,
        Helpers.GetJsonOption());

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
}
}