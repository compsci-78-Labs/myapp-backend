using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Users;
using WebApi.Domain.Projects;
using WebApi.Domain.TaskItems;

namespace WebApi.Data;

public class AppDbContext:DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project>  Projects { get; set; }
    public DbSet<TaskItem>  Tasks { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options)
        :base(options)
    {
        
    }
}