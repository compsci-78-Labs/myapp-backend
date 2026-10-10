namespace WebApi.Features.Projects;

public record ReadProjectResponse(
    Guid Id,
    string Name,
    string Description,
    Guid? Owner);