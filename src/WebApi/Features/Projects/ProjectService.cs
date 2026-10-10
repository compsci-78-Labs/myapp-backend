using WebApi.Data;
using WebApi.Domain.Projects;
using WebApi.Domain.Users;
using WebApi.Features.Users;

namespace WebApi.Features.Projects;

public class ProjectService(IProjectRepository repository,IUnitOfWork unitOfWork):IProjectService
{
 
        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            var projects = await repository.GetAllAsync();
            return projects;
        }
        public async Task<Project?> GetByIdAsync(Guid id)
        {
            var project = await repository.GetByIdAsync(id);
            return project;
        }

        public async Task<Project?> AddAsync(Project project)
        {
            await repository.AddAsync(project);
        
            await unitOfWork.SaveChangesAsync();
        
            return project;
        }

        public async Task<Project?> Update(Project projectUpdates)
        {
            var projectDb = await repository.GetByIdAsync(projectUpdates.Id);
        
            if (projectDb == null) 
                throw new Exception("project not found") ;
        
            projectDb.Name = projectUpdates.Name;
            projectDb.Description = projectUpdates.Description;
        
            await unitOfWork.SaveChangesAsync();
        
            return projectDb;
        }

        public async Task Delete(Guid id)
        {
            var projectDb = await repository.GetByIdAsync(id);
        
            if (projectDb == null) 
                throw new Exception("Project not found") ;

            repository.Delete(projectDb);
        
            await unitOfWork.SaveChangesAsync();
        }
    
}