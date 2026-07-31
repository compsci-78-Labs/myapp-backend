using WebApi.Data;
using WebApi.Data.Repositories;
using WebApi.Domain.Users;
using WebApi.Features.Project;
using WebApi.Features.Task;
using WebApi.Features.Users;

namespace WebApi.Extensions;

using Microsoft.EntityFrameworkCore;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")
            ));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskItemService, TaskItemService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
