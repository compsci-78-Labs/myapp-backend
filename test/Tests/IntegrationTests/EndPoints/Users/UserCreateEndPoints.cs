using Tests.Common;

namespace Tests.IntegrationTests.EndPoints.Users;

public class UserCreateEndPoints(WebApiFactory factory):IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateUser_ReturnsUsers_WhenUserCreated()
    {
    }
}