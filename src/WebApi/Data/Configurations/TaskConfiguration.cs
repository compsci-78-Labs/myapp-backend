using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.TaskItems;

namespace WebApi.Data.Configurations;

public class TaskConfiguration: IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.Priority)
            .HasConversion<String>()
            .HasMaxLength(20)
            .HasDefaultValue(Priority.Medium)
            .IsRequired();
        
        builder.Property(t => t.Status)
            .HasConversion<String>()
            .HasMaxLength(20)
            .HasDefaultValue(Status.ToDo)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()")
            .IsRequired();
        
        builder.HasOne(t => t.Project)
            .WithMany(p => p.TaskItems)
            .HasForeignKey(t => t.ProjectId);

        builder.HasOne(t => t.User)
            .WithMany(u => u.TaskItems)
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
}