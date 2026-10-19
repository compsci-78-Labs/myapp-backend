namespace WebApi.Presentation.Auth;

public record AuthRefreshResponse(
    string AccessToken,
    string RefreshToken);