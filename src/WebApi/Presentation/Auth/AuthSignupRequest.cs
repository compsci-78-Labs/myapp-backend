using WebApi.Domain.Users;
namespace WebApi.Presentation.Auth;

public record AuthSignupRequest(
    string Name,
    string Email,
    string Password);