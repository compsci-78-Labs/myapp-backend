namespace WebApi.Features.Projects;

public static class ProjectEndPoints
{
    public static void MapProjectEndpoints(this WebApplication app)
    {
        app.MapGet("/api/projects", () =>
        {
            
        });
        
        app.MapGet("/api/projects/{id}", (int id) =>
        {
            
        });
        app.MapGet("/api/projects/{id}/taskitems", (int id) =>
        {
            
        });
        
        app.MapPost("/api/projects", () =>
        {
            
        });
        app.MapPatch("/api/projects/{id}", (int id) =>
        {
            
        });
        
        app.MapDelete("/api/projects/{id}", (int id) =>
        {
            
        });
    }
    
}