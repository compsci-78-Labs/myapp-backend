namespace WebApi.Features.User;

public static class UserEndPoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/api/users", () =>
        {
        });
    }
}