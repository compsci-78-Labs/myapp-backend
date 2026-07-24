using WebApi.Domain.Users;
using WebApi.Domain.Projects;
namespace WebApi.Domain.TaskItems;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; }= String.Empty;
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    public Guid AssignedToId { get; set; }
    public User User { get; set; } 
}