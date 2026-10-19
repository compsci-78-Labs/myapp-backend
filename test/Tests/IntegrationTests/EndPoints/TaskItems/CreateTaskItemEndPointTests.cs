using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Projects;
using WebApi.Domain.TaskItems;
using WebApi.Domain.Users;
using WebApi.Application.TaskItems;
using WebApi.Presentation.TaskItems;
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
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);

        // Get project id
        var dbProject = await GetProject("Task API");
        dbProject.Should().NotBeNull(); // Ensure Project IS FOUND

        var dbUser = await GetUser(UserRole.User, "Alice Johnson");
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
        var response = await authClient.PostAsJsonAsync(
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