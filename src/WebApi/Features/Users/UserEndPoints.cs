namespace WebApi.Features.Users;

public static class UserEndPoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/api/users", () =>
        {
        });
    }
}