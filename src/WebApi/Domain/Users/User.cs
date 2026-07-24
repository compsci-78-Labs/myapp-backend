
namespace WebApi.Domain;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string PasswordHash { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
}