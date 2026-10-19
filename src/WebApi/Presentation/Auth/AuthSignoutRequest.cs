namespace WebApi.Presentation.Auth;

public record AuthSignoutRequest(
    String RefreshToken);