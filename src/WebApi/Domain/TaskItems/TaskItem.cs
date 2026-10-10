using WebApi.Domain.Users;
using WebApi.Domain.Projects;
namespace WebApi.Domain.TaskItems;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; }= String.Empty;
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public Guid? AssignedToId { get; set; }
    public User? User { get; set; } = null!;
}