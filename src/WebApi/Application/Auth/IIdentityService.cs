using WebApi.Domain.Users;

namespace WebApi.Application.Auth;

public interface IIdentityService
{
    Task<string> GenerateAccessToken(User user);
    Task<string> HashPassword(string password);
    Task<bool> VerifyPassword(string password, string passwordHash);
    Task<string> HashToken(string token);
    Task<string> GenerateRefreshToken();
}