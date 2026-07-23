namespace WebApi.Features.Task;

public class Task
{
    public Guid Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; }= String.Empty;
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public Guid ProjectId { get; set; }
    public Guid AssignedToId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    
}