using WebApi.Domain.TaskItems;

namespace WebApi.Features.TaskItems;

public record UpdateTaskItemRequest(
    String Title, 
    String Description,
    Status Status,
    Priority Priority,
    Guid ProjectId,
    Guid? AssignedToId);