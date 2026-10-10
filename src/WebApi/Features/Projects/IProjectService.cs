using WebApi.Domain.Projects;
using WebApi.Domain.Users;

namespace WebApi.Features.Projects;

public interface IProjectService
{
    Task<IEnumerable<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(Guid id);
    Task <Project?> AddAsync(Project user);
    Task <Project?> Update(Project user);
    Task Delete(Guid id);
}