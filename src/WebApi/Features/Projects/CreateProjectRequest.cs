namespace WebApi.Features.Projects;

public record CreateProjectRequest(
    String Name,
    String Description,
    Guid OwnerId);