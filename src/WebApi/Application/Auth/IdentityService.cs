using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using WebApi.Domain.Users;
using WebApi.Config;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WebApi.Application.Auth;

public class IdentityService(JwtConfiguration config):IIdentityService
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public async  Task<string> GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Example subject ID
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.PreferredUsername, user.Name ?? string.Empty),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config.Issuer,
            audience: config.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(config.ExpireMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshToken()
    {
        return  WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(64));
    }
    
    public async Task<string> HashToken(string token)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }

    public async Task<string> HashPassword(string password)
    {
        return  _passwordHasher.HashPassword(null!, password);
    }

    public  async Task<bool> VerifyPassword(string password, string passwordHash)
    {
        var result =  _passwordHasher.VerifyHashedPassword(
            null!,
            passwordHash,
            password);

        return result != PasswordVerificationResult.Failed;
    }
}