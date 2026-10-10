using WebApi.Domain.TaskItems;

namespace WebApi.Features.TaskItems;

public record CreateTaskItemRequest(
    String Title, 
    String Description,
    Status Status,
    Priority Priority,
    Guid ProjectId,
    Guid? AssignedToId);