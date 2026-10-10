using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Projects;

namespace WebApi.Data.Repositories;

public class ProjectRepository(AppDbContext context):IProjectRepository
{
    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await context.Projects.ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await context.Projects.FindAsync(id);
    }

    public async Task<Project?> AddAsync(Project project)
    {
        await context.Projects.AddAsync(project);

        return project;
    }

    public void Update(Project project)
    {
        context.Projects.Update(project);
    }

    public void Delete(Project project)
    {
        context.Projects.Remove(project);
    }
}