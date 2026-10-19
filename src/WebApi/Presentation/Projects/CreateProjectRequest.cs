namespace WebApi.Presentation.Projects;

public record CreateProjectRequest(
    String Name,
    String Description,
    Guid? OwnerId);