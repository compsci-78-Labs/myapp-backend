namespace WebApi.Features.Project;

public static class ProjectEndPoints
{
    public static void MapProjectEndpoints(this WebApplication app)
    {
        app.MapGet("/api/projects", () =>
        {
            
        });
    }
    
}