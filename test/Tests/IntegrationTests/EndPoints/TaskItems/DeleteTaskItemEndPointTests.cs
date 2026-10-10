using System.Net;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.TaskItems;

namespace Tests.IntegrationTests.EndPoints.TaskItems;

public class DeleteTaskItemEndPointTests(WebApiFactory factory):IClassFixture<WebApiFactory>
{
private readonly HttpClient _client = factory.CreateClient();

[Fact]
public async Task DeleteTaskItem_ShouldDeleteTaskItem_WhenExists()
{
    // Arrange
    await factory.ClearDatabaseTablesAsync();
    await factory.SeedDatabaseAsync();
        
    var testTaskItem = await TestDbHelper.GetEntityAsync<TaskItem>(
        factory.Services,
        t => t.Title == "Create landing page"
    );
    testTaskItem.Should().NotBeNull(); // Ensure TaskItem IS FOUND
        
    var response = await _client.DeleteAsync($"/api/taskitems/{testTaskItem.Id}");
        
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NoContent);
}
    
[Fact]
public async Task DeleteTaskItem_ShouldReturnNotFound_WhenNotExists()
{
    // Arrange
    var taskItemId=Guid.NewGuid();
        
    var response = await _client.DeleteAsync($"/api/taskitems/{taskItemId}");
        
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
}
}