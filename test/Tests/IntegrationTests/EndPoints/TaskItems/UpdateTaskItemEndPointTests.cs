using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Projects;
using WebApi.Domain.TaskItems;
using WebApi.Domain.Users;
using WebApi.Features.TaskItems;

namespace Tests.IntegrationTests.EndPoints.TaskItems;

public class UpdateTaskItemEndPointTests(WebApiFactory factory):IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task UpdateUser_ShouldReturnUpdatedUser_WhenSuccess()
    {
        // Arrange
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
        
        // Get project id
        var dbProject = await TestDbHelper.GetEntityAsync<Project>(
            factory.Services,
            p => p.Name == "Website Redesign"
        );
        
        dbProject.Should().NotBeNull(); // Ensure Project IS FOUND

        var dbUser = await TestDbHelper.GetEntityAsync<User>(
            factory.Services,
            u => u.Name == "Bob Smith"
        );
        
        var testTaskItem = await TestDbHelper.GetEntityAsync<TaskItem>(
            factory.Services,
            t => t.Title == "Create landing page"
        );
        
        testTaskItem.Should().NotBeNull(); // Ensure TaskItem IS FOUND
        
        var request = new UpdateTaskItemRequest(
            "Updated Name",
            "Updated description",
            Status.Completed,
            Priority.Critical,
            dbProject.Id,
            dbUser.Id);
        
        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/TaskItems/{testTaskItem.Id}", 
            request,
            Helpers.GetJsonOption());

        var updatedTaskItem = await response.Content.ReadFromJsonAsync<ReadTaskItemResponse>(Helpers.GetJsonOption());
        
        // Assert
        updatedTaskItem.Should().NotBeNull();
        updatedTaskItem.Title.Should().Be(request.Title);
        updatedTaskItem.Description.Should().Be(request.Description);
        updatedTaskItem.Status.Should().Be(request.Status);
        updatedTaskItem.Priority.Should().Be(request.Priority);
        updatedTaskItem.ProjectId.Should().Be(request.ProjectId);
        updatedTaskItem.AssignedTo.Should().Be(request.AssignedToId);
    }
    [Fact]
    public async Task UpdateTaskItem_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
        
        var taskItemId=Guid.NewGuid();
        var request = new UpdateTaskItemRequest(
            "Updated Name",
            "Updated description",
            Status.Completed,
            Priority.Critical,
            Guid.NewGuid(), 
            Guid.NewGuid());
        
        var response = await _client.PutAsJsonAsync(
            $"/api/TaskItems/{taskItemId}",
            request,
            Helpers.GetJsonOption());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}