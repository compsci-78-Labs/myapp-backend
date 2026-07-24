namespace WebApi.Domain;

public class Project
{
    public Guid  Id{ get; set; }
    public string Name { get; set; } = String.Empty;
    public string Description {get; set; } = String.Empty;
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt  {get; set; }
}