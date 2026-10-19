namespace WebApi.Presentation.Projects;

public record UpdateProjectRequest(
    string Name,
    string Description,
    Guid? Qwner );