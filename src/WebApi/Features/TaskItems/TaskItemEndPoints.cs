namespace WebApi.Features.TaskItems;

public static class TaskItemEndPoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        app.MapGet("/api/taskitems", () =>
        {
            
        });
    }
}