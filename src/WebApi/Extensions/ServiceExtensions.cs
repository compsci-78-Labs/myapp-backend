using WebApi.Data;
using WebApi.Features.Project;
using WebApi.Features.Task;
using WebApi.Features.User;

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

        return services;
    }
}
