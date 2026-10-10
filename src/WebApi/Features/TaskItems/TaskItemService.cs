using WebApi.Data;
using WebApi.Domain.TaskItems;
using WebApi.Domain.Users;

namespace WebApi.Features.TaskItems;

public class TaskItemService(
    ITaskItemRepository repository,
    IUnitOfWork unitOfWork):ITaskItemService
{
    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        var taskItems = await repository.GetAllAsync();
        return taskItems;
    }
    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        var taskItem = await repository.GetByIdAsync(id);
        return taskItem;
    }

    public async Task<TaskItem?> AddAsync(TaskItem taskItem)
    {
        await repository.AddAsync(taskItem);
        
        await unitOfWork.SaveChangesAsync();
        
        return taskItem;
    }

    public async Task<TaskItem?> Update(TaskItem taskItem)
    {
        var foundTaskItem = await repository.GetByIdAsync(taskItem.Id);
        
        if (foundTaskItem == null) 
            throw new Exception("TaskItem not found") ;
        
        foundTaskItem.Title = taskItem.Title;
        foundTaskItem.Description = taskItem.Description;
        foundTaskItem.Status = taskItem.Status;
        foundTaskItem.Priority = taskItem.Priority;
        foundTaskItem.ProjectId = taskItem.ProjectId;
        foundTaskItem.AssignedToId = taskItem.AssignedToId;
        
        repository.Update(foundTaskItem);
        
        await unitOfWork.SaveChangesAsync();
        
        return foundTaskItem;
    }

    public async Task Delete(Guid id)
    {
        var foundTaskItem = await repository.GetByIdAsync(id);
        
        if (foundTaskItem == null) 
            throw new Exception("TaskItem not found") ;

        repository.Delete(foundTaskItem);
        
        await unitOfWork.SaveChangesAsync();
    }
}