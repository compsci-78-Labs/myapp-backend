using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Tests.Common;
using WebApi.Domain.Users;
using WebApi.Features.Projects;

namespace Tests.IntegrationTests.EndPoints.Projects;

public class ProjectCreateEndPointTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateProject_ReturnsCreatedProject_WhenSuccess()
    {
        // Arrange
        await factory.ClearDatabaseTablesAsync();
        await factory.SeedDatabaseAsync();
        
        // Creating a projcet requires existing user in the db
        var dbUser = await TestDbHelper.GetEntityAsync<User>(
            factory.Services,
            u => u.Name == "Alice Johnson"
        );
        dbUser.Should().NotBeNull(); // Ensure USER IS FOUND
        
        var request = new CreateProjectRequest(
            "Some project",
            "Some project description",
            dbUser!.Id
        );

        var jsonOptions = Helpers.GetJsonOption();
        
        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/projects",
            request,
            jsonOptions
        );

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadFromJsonAsync<ReadProjectResponse>(jsonOptions);

        // Assert
        responseBody.Should().NotBeNull();
        responseBody!.Id.Should().NotBeEmpty();
        responseBody.Name.Should().Be("Some project");
        responseBody.Description.Should().Be("Some project description");
        responseBody.Owner.Should().Be(dbUser.Id);
    }
}