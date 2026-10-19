using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.TaskItems;
using WebApi.Application.TaskItems;
using WebApi.Domain.Projects;
using WebApi.Domain.Users;
using WebApi.Presentation.TaskItems;

namespace Tests.IntegrationTests.EndPoints.TaskItems;

public class ReadTaskItemEndPointTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

     [Fact]
    public async Task GetTaskItems_ReturnsTaskItems_WhenTaskItemsExist()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        
        // Act
        var response = await authClient.GetAsync("/api/taskitems");
        var taskItems = await response.Content.ReadFromJsonAsync<List<ReadTaskItemResponse>>(Helpers.GetJsonOption());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        taskItems.Should().NotBeNull();
        taskItems.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task GetTaskItems_ReturnsEmptyList_WhenTaskItemsDoseNotExist()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        await factory.ClearDatabaseTablesAsync();
    
        // Act
        var response = await authClient.GetAsync("/api/taskitems");
        var taskItems = await response.Content.ReadFromJsonAsync<List<ReadTaskItemResponse>>(Helpers.GetJsonOption());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        taskItems.Should().NotBeNull();
        taskItems.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetTaskItemById_ReturnsTaskItem_WhenExists()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);

        var testTaskItem = await GetTaskItem("Create landing page");
        testTaskItem.Should().NotBeNull(); // Ensure taskItem IS FOUND
        
        // Act
        var response = await authClient.GetAsync($"/api/taskitems/{testTaskItem.Id}");
        var foundTaskItem = await response.Content.ReadFromJsonAsync<ReadTaskItemResponse>(Helpers.GetJsonOption());
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        foundTaskItem.Should().NotBeNull();
        foundTaskItem.Id.Should().Be(testTaskItem.Id);
        foundTaskItem.Title.Should().Be(testTaskItem.Title);
    }
    
    [Fact]
    public async Task GetTaskItemById_ReturnsNotFound_WhenNotExists()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        var taskItemId = Guid.NewGuid();
        
        // Act
        var response = await authClient.GetAsync($"/api/taskitems/{taskItemId}");
        
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