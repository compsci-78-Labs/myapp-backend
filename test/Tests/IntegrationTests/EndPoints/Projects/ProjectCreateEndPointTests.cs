using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tests.Common;
using WebApi.Application.Auth;
using WebApi.Domain.Users;
using WebApi.Application.Projects;
using WebApi.Config;
using WebApi.Presentation.Projects;

namespace Tests.IntegrationTests.EndPoints.Projects;

public class ProjectCreateEndPointTests(WebApiFactory factory) : IClassFixture<WebApiFactory>
{

    [Fact]
    public async Task CreateProject_ReturnsCreatedProject_WhenSuccess()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        
        var projectOwner = await GetUser(UserRole.User, "Alice Johnson");
        projectOwner.Should().NotBeNull(); // Ensure USER IS FOUND
        
        var request = new CreateProjectRequest(
            "Some project",
            "Some project description",
            projectOwner!.Id
        );
        
        var jsonOptions = Helpers.GetJsonOption();
        
        // Act
        var response = await authClient.PostAsJsonAsync(
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
        responseBody.Owner.Should().Be(projectOwner.Id);
    }
    
    private async Task<HttpClient> GetAuthenticatedHttpClient(UserRole user)
    {
        var authUser = await TestDbHelper.GetEntityAsync<User>(
            factory.Services,
            u => u.Role == user);

        return await factory.CreateAuthenticatedClientAsync(authUser!);
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