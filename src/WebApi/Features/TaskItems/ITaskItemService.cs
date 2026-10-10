using WebApi.Domain.TaskItems;

namespace WebApi.Features.TaskItems;

public interface ITaskItemService
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task <TaskItem?> AddAsync(TaskItem taskItem);
    Task <TaskItem?> Update(TaskItem taskItem);
    Task Delete(Guid id);
}