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

        public async Task<Project?> Update(Project project)
        {
            var foundProject = await repository.GetByIdAsync(project.Id);
        
            if (foundProject == null) 
                throw new Exception("project not found") ;
        
            foundProject.Name = project.Name;
            foundProject.Description = project.Description;
            foundProject.OwnerId = project.OwnerId;
        
            await unitOfWork.SaveChangesAsync();
        
            return foundProject;
        }

        public async Task Delete(Guid id)
        {
            var foundProject = await repository.GetByIdAsync(id);
        
            if (foundProject == null) 
                throw new Exception("Project not found") ;

            repository.Delete(foundProject);
        
            await unitOfWork.SaveChangesAsync();
        }
    
}