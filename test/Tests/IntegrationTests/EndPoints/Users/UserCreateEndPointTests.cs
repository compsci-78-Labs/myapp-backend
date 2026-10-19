using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Tests.Common;
using WebApi.Domain.Users;
using WebApi.Application.Users;
using WebApi.Domain.Projects;
using WebApi.Presentation.Users;
using WebApi.Serialization;
using Xunit.Abstractions;

namespace Tests.IntegrationTests.EndPoints.Users;

public class UserCreateEndPointTests(
    WebApiFactory factory, 
    ITestOutputHelper output
    ) : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    

    [Fact]
    public async Task CreateUser_ReturnsCreatedUser_WhenSuccess()
    {
        // Arrange
        await SeedDatabase();
        var authClient = await GetAuthenticatedHttpClient(UserRole.Manager);
        
        var request = new CreateUserRequest(
            "John Doe",
            "john.doe@example.com",
            UserRole.User
        );

        var jsonOptions = Helpers.GetJsonOption();
            
        // Act
        var response = await authClient.PostAsJsonAsync(
            "/api/users",
            request,
            jsonOptions
        );

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadAsStringAsync();

        var createdUser = JsonSerializer.Deserialize<ReadUserResponse>(
            responseBody,
            jsonOptions
        );
        
        // Assert
        createdUser.Should().NotBeNull();
        createdUser!.Id.Should().NotBeEmpty();
        createdUser.Name.Should().Be("John Doe");
        createdUser.Email.Should().Be("john.doe@example.com");
        createdUser.Role.Should().Be(UserRole.User);
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