namespace WebApi.Features.Task;

public static class TaskItemEndPoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        app.MapGet("/api/taskitems", () =>
        {
            
        });
    }
}