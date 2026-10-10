using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.TaskItems;
using WebApi.Features.TaskItems;

namespace Tests.IntegrationTests.EndPoints.TaskItems;

public class ReadTaskItemEndPointTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

     [Fact]
    public async Task GetTaskItems_ReturnsTaskItems_WhenTaskItemsExist()
    {
        // Arrange
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
        
        // Act
        var response = await _client.GetAsync("/api/taskitems");
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
        await factory.ClearDatabaseTablesAsync();
    
        // Act
        var response = await _client.GetAsync("/api/taskitems");
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
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
        
        var testTaskItem = await TestDbHelper.GetEntityAsync<TaskItem>(
            factory.Services,
            u => u.Title == "Create landing page"
            );
        testTaskItem.Should().NotBeNull(); // Ensure taskItem IS FOUND
        
        // Act
        var response = await _client.GetAsync($"/api/taskitems/{testTaskItem.Id}");
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
        var taskItemId = Guid.NewGuid();
        
        // Act
        var response = await _client.GetAsync($"/api/taskitems/{taskItemId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}