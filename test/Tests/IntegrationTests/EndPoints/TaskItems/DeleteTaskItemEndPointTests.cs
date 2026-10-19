using System.Net;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Projects;
using WebApi.Domain.TaskItems;
using WebApi.Domain.Users;

namespace Tests.IntegrationTests.EndPoints.TaskItems;

public class DeleteTaskItemEndPointTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task DeleteTaskItem_ShouldDeleteTaskItem_WhenExists()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);

        var testTaskItem = await GetTaskItem("Create landing page");
        testTaskItem.Should().NotBeNull(); // Ensure TaskItem IS FOUND

        var response = await authClient.DeleteAsync($"/api/taskitems/{testTaskItem.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteTaskItem_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        var taskItemId = Guid.NewGuid();

        var response = await _client.DeleteAsync($"/api/taskitems/{taskItemId}");

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