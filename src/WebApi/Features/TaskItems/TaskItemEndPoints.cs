using System.Diagnostics.Eventing.Reader;
using WebApi.Domain.TaskItems;

namespace WebApi.Features.TaskItems;

public static class TaskItemEndPoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        app.MapGet("/api/taskitems", async (ITaskItemService service) =>
        {
            var taskItems = await service.GetAllAsync();
            
            var taskItemsResponse = new List<ReadTaskItemResponse>();
            
            foreach (var taskItem in taskItems)
            {
                taskItemsResponse.Add(new ReadTaskItemResponse(
                    taskItem.Id,
                    taskItem.Title,
                    taskItem.Description,
                    taskItem.Status,
                    taskItem.Priority,
                    taskItem.ProjectId,
                    taskItem.AssignedToId));
            }

            return Results.Ok(taskItemsResponse);
        });
        
        app.MapGet("/api/taskitems/{id}", async (ITaskItemService service, Guid id) =>
        {
            var taskItem = await service.GetByIdAsync(id);
            return taskItem != null
                ? Results.Ok(new ReadTaskItemResponse(
                    taskItem.Id,
                    taskItem.Title,
                    taskItem.Description,
                    taskItem.Status,
                    taskItem.Priority,
                    taskItem.ProjectId,
                    taskItem.AssignedToId
                ))
                : Results.NotFound();
        });
        
        app.MapPost("/api/taskitems", async (ITaskItemService service, CreateTaskItemRequest request) =>
        {
            var taskItem = new TaskItem()
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                Priority = request.Priority,
                ProjectId =  request.ProjectId,
                AssignedToId = request.AssignedToId
            };
            
            var createdTaskItem = await service.AddAsync(taskItem );
  
            return Results.Created(
                $"/api/taskitems/{createdTaskItem?.Id}",
                new ReadTaskItemResponse(
                    createdTaskItem.Id,
                    createdTaskItem.Title,
                    createdTaskItem.Description,
                    createdTaskItem.Status,
                    createdTaskItem.Priority,
                    createdTaskItem.ProjectId,
                    createdTaskItem.AssignedToId
                ));
        });
        
        app.MapPut("/api/taskitems/{id}", async (Guid id, ITaskItemService service, UpdateTaskItemRequest request) =>
        {
            var taskItem = await service.GetByIdAsync(id);

            if (taskItem == null) return Results.NotFound("TaskItem not found");
            
            taskItem.Title = request.Title;
            taskItem.Description = request.Description;
            taskItem.Status = request.Status;
            taskItem.Priority = request.Priority;
            taskItem.ProjectId = request.ProjectId;
            taskItem.AssignedToId = request.AssignedToId;
            
            var updatedTaskItem = await service.Update(taskItem);

            return Results.Ok(new ReadTaskItemResponse(
                updatedTaskItem.Id,
                updatedTaskItem.Title,
                updatedTaskItem.Description,
                updatedTaskItem.Status,
                updatedTaskItem.Priority,
                updatedTaskItem.ProjectId,
                updatedTaskItem.AssignedToId
            ));
        });

        app.MapDelete("/api/taskitems/{id}", async (ITaskItemService service, Guid id) =>
        {

            var taskItem = await service.GetByIdAsync(id);

            if (taskItem == null) return Results.NotFound("TaskItem not found");

            await service.Delete(id);

            return Results.NoContent();
        });
    }
}