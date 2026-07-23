namespace WebApi.Features.Project;

public static class ProjectEndPoints
{
    public static void MapProjectEndpoints(this WebApplication app)
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