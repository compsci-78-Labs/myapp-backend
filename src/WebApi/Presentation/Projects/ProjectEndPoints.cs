using WebApi.Domain.Projects;
using WebApi.Application.Projects;
using WebApi.Application.Users;

namespace WebApi.Presentation.Projects;

public static class ProjectEndPoints
{
    public static void MapProjectEndpoints(this WebApplication app)
    {
        app.MapGet("/api/projects", async (IProjectService service) =>
        {
            var projects = await service.GetAllAsync();
            var projectsArray = new List<ReadProjectResponse>();
            foreach (var project in projects)
            {
                projectsArray.Add(new ReadProjectResponse(
                    project.Id,
                    project.Name,
                    project.Description,
                    project.OwnerId
                ));
            }

            return Results.Ok(projectsArray);
            
        }).RequireAuthorization();
        
        app.MapGet("/api/projects/{id}", async (IProjectService service, Guid id) =>
        {
            var project = await service.GetByIdAsync(id);
            return project != null
                ? Results.Ok(new ReadProjectResponse(
                    project.Id,
                    project.Name,
                    project.Description,
                    project.OwnerId
                ))
                : Results.NotFound();
        }).RequireAuthorization();
        
        app.MapGet("/api/projects/{id}/taskitems", async (IProjectService service, Guid id) =>
        {
            
        }).RequireAuthorization();
        
        app.MapPost("/api/projects", async (IProjectService service, CreateProjectRequest request) =>
        {
            var projectToBeCreated = new Project()
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = request.OwnerId
            };
            
            var project = await service.AddAsync(projectToBeCreated );
  
            return Results.Created(
                $"/api/projects/{project?.Id}",
                new ReadProjectResponse(
                    project.Id,
                    project.Name,
                    project.Description,
                    project.OwnerId
                ));
        }).RequireAuthorization();
        
        app.MapPut("/api/projects/{id}", async (IProjectService service,Guid id, UpdateProjectRequest request) =>
        {
            
                var project = await service.GetByIdAsync(id);

                if (project == null) return Results.NotFound("Project not found");
            
                project.Name = request.Name;
                project.Description = request.Description;
                project.OwnerId = request.Qwner;
            
                var updatedProject = await service.Update(project);

                return Results.Ok(new ReadProjectResponse(
                    updatedProject.Id,
                    updatedProject.Name,
                    updatedProject.Description,
                    updatedProject.OwnerId
                ));
            
            
        }).RequireAuthorization();
        
        app.MapDelete("/api/projects/{id}", async (IProjectService service,Guid id) =>
        {
            var project = await service.GetByIdAsync(id);

            if (project == null) return Results.NotFound("Project not found");
            
            await service.Delete(id);

            return Results.NoContent();
        }).RequireAuthorization();
    }
    
}