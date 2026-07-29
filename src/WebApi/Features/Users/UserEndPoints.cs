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
    }
}