namespace WebApi.Features.Task;

public static class TaskItemEndPoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        app.MapGet("/projects", () =>
        {
            return Results.Ok(new[]
            {
                new { Id = 1, Name = "Laptop" }
            });
        });
    }
}