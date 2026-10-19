namespace WebApi.Presentation.Auth;

public record AuthSigninRequest(
    string Email,
    string Password);