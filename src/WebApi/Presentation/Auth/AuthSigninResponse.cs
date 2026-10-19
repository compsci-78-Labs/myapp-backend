using WebApi.Presentation.Users;

namespace WebApi.Presentation.Auth;

public record AuthSigninResponse(
    string AccessToken,
    string RefreshToken,
    ReadUserResponse User);