using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Domain.Projects;
using WebApi.Domain.TaskItems;
using WebApi.Domain.Users;

namespace Tests.Common;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        db.Database.EnsureCreated();
        
        if (db.Users.Any()) return;

        // Users
        var alice = new User
        {
            Id = Guid.NewGuid(),
            Name = "Alice Johnson",
            Email = "alice@example.com",
            PasswordHash = "hashed-password-1"
        };

        var bob = new User
        {
            Id = Guid.NewGuid(),
            Name = "Bob Smith",
            Email = "bob@example.com",
            PasswordHash = "hashed-password-2"
        };

        db.Users.AddRange(alice, bob);

        // Projects
        var websiteProject = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Website Redesign",
            Description = "Redesign the company website.",
            OwnerId = alice.Id,
            Owner = alice,
            CreatedAt = DateTime.UtcNow
        };

        var apiProject = new Project
        {
            Id = Guid.NewGuid(),
            Name = "Task API",
            Description = "Develop the task management API.",
            OwnerId = bob.Id,
            Owner = bob,
            CreatedAt = DateTime.UtcNow
        };

        db.Projects.AddRange(websiteProject, apiProject);


        // Tasks
        var task1 = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Create landing page",
            Description = "Design and implement the landing page.",
            Status = Status.InProgress,
            Priority = Priority.High,
            ProjectId = websiteProject.Id,
            Project = websiteProject,
            AssignedToId = alice.Id,
            User = alice
        };

        var task2 = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Implement authentication",
            Description = "Add JWT authentication.",
            Status = Status.ToDo,
            Priority = Priority.High,
            ProjectId = apiProject.Id,
            Project = apiProject,
            AssignedToId = bob.Id,
            User = bob
        };

        var task3 = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Write unit tests",
            Description = "Cover services with unit tests.",
            Status = Status.Completed,
            Priority = Priority.Medium,
            ProjectId = apiProject.Id,
            Project = apiProject,
            AssignedToId = alice.Id,
            User = alice
        };

        db.TaskItems.AddRange(task1, task2, task3);

        db.SaveChanges();
    }
}