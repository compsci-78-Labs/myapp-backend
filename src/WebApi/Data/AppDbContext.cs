using Microsoft.EntityFrameworkCore;
using WebApi.Features.Project;
using WebApi.Features.User;

namespace WebApi.Data;

public class AppDbContext:DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project>  Projects { get; set; }
    public DbSet<Task>  Tasks { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options)
        :base(options)
    {
        
    }
}