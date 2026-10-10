namespace WebApi.Features.Projects;

public record UpdateProjectRequest(
    string Name,
    string Description,
    Guid? Qwner );