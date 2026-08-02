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
            return user != null ? Results.Ok(user) : Results.NotFound();
        });
        
        app.MapPost("/api/users",async (IUserService service, User newUser ) =>
        {
            var user = await service.AddAsync(newUser);
            
            return Results.Created($"/api/users/{user?.Id}", user);
            
        });
        
        app.MapPut("/api/users/{id}",async (IUserService service, User userUpdates ) =>
        {
            var user = await service.Update(userUpdates);
            
            return Results.Ok(user);
            
        });
        
        app.MapDelete("/api/users/{id}",async (IUserService service, Guid id) =>
        {
            await service.Delete(id);
            
            return Results.NoContent();
            
        });
    }
}