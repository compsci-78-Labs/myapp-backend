namespace WebApi.Domain.Projects;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(Guid id);
    Task <Project?> AddAsync(Project project);
    void Delete(Project project);
}