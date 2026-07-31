using WebApi.Domain.Users;

namespace WebApi.Features.Users;

public static class UserEndPoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/api/users",async (IUserService service ) =>
        {
            var users = await service.GetAllAsync();

            return Results.Ok(users);

        });
        
        app.MapGet("/api/users/{id}",async (IUserService service, Guid id ) =>
        {
            var user = await service.GetByIdAsync(id);
            
            return Results.Ok(user);
        });
        
        app.MapPost("/api/users",async (IUserService service, User newUser ) =>
        {
            
        });
    }
}