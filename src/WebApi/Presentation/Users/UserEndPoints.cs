using WebApi.Domain.Users;
using WebApi.Application.Users;

namespace WebApi.Presentation.Users;

public static class UserEndPoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/api/users", async (IUserService service) =>
        {
            var users = await service.GetAllAsync();
            var usersArray = new List<ReadUserResponse>();
            foreach (var user in users)
            {
                usersArray.Add(new ReadUserResponse(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role
                ));
            }

            return Results.Ok(usersArray);
        }).RequireAuthorization();

        app.MapGet("/api/users/{id}", async (IUserService service, Guid id) =>
        {
            var user = await service.GetByIdAsync(id);
            return user != null
                ? Results.Ok(new ReadUserResponse(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role
                ))
                : Results.NotFound();
        }).RequireAuthorization();


        app.MapPost("/api/users", async (IUserService service, CreateUserRequest request) =>
        {
            var userToBeCreated = new User()
            {
                Name = request.Name,
                Email = request.Email,
                Role = request.Role
            };
            try
            {
                var user = await service.AddAsync(userToBeCreated );
                
                return Results.Created(
                    $"/api/users/{user?.Id}",
                    new ReadUserResponse(
                        user.Id,
                        user.Name,
                        user.Email,
                        user.Role
                    ));
            }
            catch (Exception e)
            {
                return Results.Problem(
                    title:"User creation failed",
                    detail:e.Message,
                    statusCode: StatusCodes.Status500InternalServerError);
            }
            
        }).RequireAuthorization();


        app.MapPut("/api/users/{id}", async (Guid id, IUserService service, UpdateUserRequest request) =>
        {
            var user = await service.GetByIdAsync(id);

            if (user == null) return Results.NotFound("User not found");
            
            user.Name = request.Name;
            user.Email = request.Email;
            user.Role = request.Role;
            
            var updatedUser = await service.Update(user);

            return Results.Ok(new ReadUserResponse(
                updatedUser.Id,
                updatedUser.Name,
                updatedUser.Email,
                updatedUser.Role
                ));
        }).RequireAuthorization();

        app.MapDelete("/api/users/{id}", async (IUserService service, Guid id) =>
        {
            var user = await service.GetByIdAsync(id);

            if (user == null) return Results.NotFound("User not found");
            
            await service.Delete(id);

            return Results.NoContent();
        }).RequireAuthorization();
    }
}