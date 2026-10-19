namespace WebApi.Presentation.Projects;

public record ReadProjectResponse(
    Guid Id,
    string Name,
    string Description,
    Guid? Owner);