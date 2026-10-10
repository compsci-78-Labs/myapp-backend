namespace WebApi.Domain.TaskItems;

public interface ITaskItemRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task <TaskItem?> AddAsync(TaskItem taskItem);
    void Update(TaskItem user);
    void Delete(TaskItem user);
}