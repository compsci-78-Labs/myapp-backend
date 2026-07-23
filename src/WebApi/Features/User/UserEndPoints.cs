namespace WebApi.Features.User;

public static class UserEndPoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/tasks", () =>
        {
            return Results.Ok(new[]
            {
                new { Id = 1, Name = "Laptop" }
            });
        });
    }
}