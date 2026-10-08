using System.Diagnostics.Eventing.Reader;

namespace WebApi.Features.TaskItems;

public static class TaskItemEndPoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        app.MapGet("/api/taskitems", () =>
        {
            
        });
        
        app.MapGet("/api/taskitems/{id}", (int id) =>
        {
            
        });
        
        app.MapPost("/api/taskitems", () =>
        {
            
        });
        
        app.MapPost("/api/taskitems/{id}", (int id) =>
        {
            
        });
        
        app.MapDelete("/api/taskitems/{id}", (int id) =>
        {
            
        });

    }
}