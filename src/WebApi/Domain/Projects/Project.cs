using WebApi.Domain.TaskItems;
using WebApi.Domain.Users;

namespace WebApi.Domain.Projects;

public class Project
{
    public Guid  Id{ get; set; } = Guid.NewGuid();
    public string Name { get; set; } = String.Empty;
    public string Description {get; set; } = String.Empty;
    public Guid? OwnerId { get; set; } = null;
    public User? Owner { get; set; } = null!;
    public DateTime CreatedAt  {get; set; }
    
    public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
}