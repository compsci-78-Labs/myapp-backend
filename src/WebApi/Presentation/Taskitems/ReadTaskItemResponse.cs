using WebApi.Domain.TaskItems;

namespace WebApi.Presentation.TaskItems;

public record ReadTaskItemResponse(
    Guid Id,
    string Title,
    string Description,
    Status Status,
    Priority Priority,
    Guid ProjectId,
    Guid? AssignedTo);