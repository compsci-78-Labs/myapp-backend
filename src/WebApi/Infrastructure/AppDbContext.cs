using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Users;
using WebApi.Domain.Projects;
using WebApi.Domain.TaskItems;

namespace WebApi.Infrastructure;

public class AppDbContext:DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project>  Projects { get; set; }
    public DbSet<TaskItem>  TaskItems { get; set; }
    public DbSet<RefreshToken>  RefreshTokens { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options)
        :base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}