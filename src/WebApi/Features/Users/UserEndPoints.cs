using WebApi.Domain.Users;

namespace WebApi.Features.Users;

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
        });

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
        });


        app.MapPost("/api/users", async (IUserService service, CreateUserRrequest request) =>
        {
            var userToBeCreated = new User()
            {
                Name = request.Name,
                Email = request.Email,
                Role = request.Role
            };
            
            var user = await service.AddAsync(userToBeCreated );
  
            return Results.Created(
                $"/api/users/{user?.Id}",
                new ReadUserResponse(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Role
                ));
        });


        app.MapPut("/api/users/{id}", async (Guid id, IUserService service, UpdateUserRequest request) =>
        {
            var user = await service.GetByIdAsync(id);

            if (user == null) return Results.NotFound("User not found");
            
            user.Name = request.Name;
            user.Email = request.Email;
            user.Role = request.Role;
            
            var updatedUser = await service.Update(user);

            return Results.Ok(new ReadUserResponse(
                user.Id,
                user.Name,
                user.Email,
                user.Role
                ));
        });

        app.MapDelete("/api/users/{id}", async (IUserService service, Guid id) =>
        {
            
            var user = await service.GetByIdAsync(id);

            if (user == null) return Results.NotFound("User not found");
            
            await service.Delete(id);

            return Results.NoContent();
        });
    }
}