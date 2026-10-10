using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Projects;
using WebApi.Domain.TaskItems;
using WebApi.Domain.Users;
using WebApi.Features.TaskItems;
using Xunit.Abstractions;

namespace Tests.IntegrationTests.EndPoints.TaskItems;

public class CreateTaskItemEndPointTests(
    WebApiFactory factory,
    ITestOutputHelper output
) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();


    [Fact]
    public async Task CreateTaskItem_ReturnsCreatedTaskItem_WhenSuccess()
    {
        // Arrange
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();

        // Get project id
        var dbProject = await TestDbHelper.GetEntityAsync<Project>(
            factory.Services,
            p => p.Name == "Task API"
        );
        dbProject.Should().NotBeNull(); // Ensure Project IS FOUND

        var dbUser = await TestDbHelper.GetEntityAsync<User>(
            factory.Services,
            u => u.Name == "Alice Johnson"
        );
        dbUser.Should().NotBeNull(); // Ensure USER IS FOUND

        var request = new CreateTaskItemRequest(
            "New task",
            "New task description",
            Status.ToDo,
            Priority.Low,
            dbProject.Id,
            dbUser.Id
        );

        var jsonOptions = Helpers.GetJsonOption();

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/taskitems",
            request,
            jsonOptions
        );
        
        //response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadAsStringAsync();
        
        var createdTaskItem = JsonSerializer.Deserialize<ReadTaskItemResponse>(
            responseBody,
            jsonOptions
        );

        // Assert
        createdTaskItem.Should().NotBeNull();
        createdTaskItem!.Id.Should().NotBeEmpty();
        createdTaskItem.Title.Should().Be(request.Title);
        createdTaskItem.Description.Should().Be(request.Description);
        createdTaskItem.Status.Should().Be(request.Status);
        createdTaskItem.ProjectId.Should().Be(request.ProjectId);
        createdTaskItem.AssignedTo.Should().Be(request.AssignedToId);
    }
}