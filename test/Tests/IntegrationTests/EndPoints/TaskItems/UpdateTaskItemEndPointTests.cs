using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Projects;
using WebApi.Domain.TaskItems;
using WebApi.Domain.Users;
using WebApi.Application.TaskItems;
using WebApi.Presentation.TaskItems;

namespace Tests.IntegrationTests.EndPoints.TaskItems;

public class UpdateTaskItemEndPointTests(WebApiFactory factory):IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task UpdateUser_ShouldReturnUpdatedUser_WhenSuccess()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        
        // Get project id
        var dbProject = await GetProject("Website Redesign");
        dbProject.Should().NotBeNull(); // Ensure Project IS FOUND
        
        var dbUser = await GetUser(UserRole.User, "Bob Smith");
        dbUser.Should().NotBeNull();
        
        var testTaskItem = await GetTaskItem("Create landing page");
        testTaskItem.Should().NotBeNull(); // Ensure TaskItem IS FOUND
        
        var request = new UpdateTaskItemRequest(
            "Updated Name",
            "Updated description",
            Status.Completed,
            Priority.Critical,
            dbProject.Id,
            dbUser.Id);
        
        // Act
        var response = await authClient.PutAsJsonAsync(
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
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);;
        
        var taskItemId=Guid.NewGuid();
        var request = new UpdateTaskItemRequest(
            "Updated Name",
            "Updated description",
            Status.Completed,
            Priority.Critical,
            Guid.NewGuid(), 
            Guid.NewGuid());
        
        var response = await authClient.PutAsJsonAsync(
            $"/api/TaskItems/{taskItemId}",
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
    private async Task<TaskItem?> GetTaskItem(string title)
    {
        return await TestDbHelper.GetEntityAsync<TaskItem>(
            factory.Services,
            p =>  p.Title == title);
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