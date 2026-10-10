using WebApi.Domain.TaskItems;

namespace WebApi.Features.TaskItems;

public record ReadTaskItemResponse(
    Guid Id,
    string Title,
    string Description,
    Status Status,
    Priority Priority,
    Guid ProjectId,
    Guid? AssignedTo);