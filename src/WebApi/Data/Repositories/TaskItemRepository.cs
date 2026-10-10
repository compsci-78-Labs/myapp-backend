using Microsoft.EntityFrameworkCore;
using WebApi.Domain.TaskItems;

namespace WebApi.Data.Repositories;

public class TaskItemRepository(AppDbContext context):ITaskItemRepository
{
    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await context.TaskItems.ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await context.TaskItems.FindAsync(id);
    }

    public async Task<TaskItem?> AddAsync(TaskItem taskItem)
    {
        var projectExists = await context.Projects
            .AnyAsync(p => p.Id == taskItem.ProjectId);

        var userExists = await context.Users
            .AnyAsync(u => u.Id == taskItem.AssignedToId);

        Console.WriteLine($"Project exists: {projectExists}");
        Console.WriteLine($"User exists: {userExists}");
        
        await context.TaskItems.AddAsync(taskItem);

        return taskItem;
    }

    public  void Update(TaskItem taskItem)
    {
        context.TaskItems.Update(taskItem);
    }

    public void Delete(TaskItem taskItem)
    {
        context.TaskItems.Remove(taskItem);
    }
}