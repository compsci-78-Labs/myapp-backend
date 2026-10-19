using System.Security.Claims;
using WebApi.Domain.Users;
using WebApi.Application.Auth;
using WebApi.Application.Users;
using WebApi.Extensions;
using WebApi.Presentation.Users;

namespace WebApi.Presentation.Auth;

public static class AuthEndPoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/signup", async (
            IAuthService authService,
            AuthSignupRequest request) =>
        {
            try
            {
                await authService.Signup(request);
                return Results.Created(
                    "Aapi/auth/signin",
                    new { message = "Account created successfully" });
            }
            catch (Exception e)
            {
                return Results.Problem(
                    title: "User registration failed",
                    detail: e.Message,
                    statusCode:
                    StatusCodes.Status500InternalServerError);
            }
        });


        app.MapPost("/api/auth/signin", async (
            HttpContext context,
            IAuthService authService,
            AuthSigninRequest request) =>
        {
            try
            {
                var signinResponse = await authService.Signin(request);

                context.Response.SetRefreshTokenCookie(signinResponse.RefreshToken);

                return Results.Ok(new
                {
                    message = "Success",
                    accessToken = signinResponse.AccessToken,
                    user = signinResponse.User,
                });
            }
            catch (Exception e)
            {
                return Results.Problem(
                    title: "User signin failed",
                    detail: e.Message,
                    statusCode:
                    StatusCodes.Status500InternalServerError);
            }
        });

        app.MapGet("/api/auth/me", async (IAuthService service, HttpContext context) =>
        {
            try
            {
                if (context.User.Identity?.IsAuthenticated != true)
                    return Results.Unauthorized();

                var stringId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (stringId == null)
                    return Results.Unauthorized();

                if (!Guid.TryParse(stringId, out Guid userId))
                    return Results.Unauthorized();

                var user = await service.Me(userId);

                return Results.Ok(new
                {
                    user = new ReadUserResponse
                    (
                        user.Id,
                        user.Name,
                        user.Email,
                        user.Role
                    )
                });
            }
            catch (Exception e)
            {
                return Results.Problem(
                    title: "Get user failed",
                    detail: e.Message,
                    statusCode:
                    StatusCodes.Status500InternalServerError);
            }
        }).RequireAuthorization();

        app.MapPost("/api/auth/signout", async (
            HttpContext context,
            IAuthService service) =>
        {
            try
            {
                var refreshToken = context.Request.Cookies["refreshToken"];

                if (!string.IsNullOrEmpty(refreshToken))
                    await service.Signout(refreshToken);

                context.Response.ClearRefreshTokenCookie();

                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.Problem(
                    title: "Signout failed",
                    detail: e.Message,
                    statusCode:
                    StatusCodes.Status500InternalServerError);
            }
        }).RequireAuthorization();

        app.MapPost("/api/auth/refresh", async (
            HttpContext context,
            IAuthService service) =>
        {
            try
            {
                var refreshToken = context.Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                    return Results.Unauthorized();
                
                var refreshResponse = await service.Refresh(refreshToken);
                context.Response.SetRefreshTokenCookie(refreshResponse.RefreshToken);

                return Results.Ok(new
                    {
                        message = "Success",
                        accessToken = refreshResponse.AccessToken
                    }
                );
            }
            catch (Exception e)
            {
                return Results.Problem(
                    title: "Token refresh failed.",
                    detail: e.Message,
                    statusCode:
                    StatusCodes.Status500InternalServerError);
            }
        });
    }
}