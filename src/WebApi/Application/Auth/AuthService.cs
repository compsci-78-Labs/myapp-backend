using WebApi.Domain.Users;
using WebApi.Application.Users;
using WebApi.Infrastructure;
using WebApi.Presentation.Auth;
using WebApi.Presentation.Users;

namespace WebApi.Application.Auth;

public class AuthService(
    IUserRepository userRepository,
    IIdentityService identityService,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork
    ) : IAuthService
{
    public async Task Signup(AuthSignupRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            throw new Exception("Email and password are required.");

        var hashPassword = await identityService.HashPassword(request.Password);

        var foundUser = await userRepository.GetByEmailAsync(request.Email);

        if (foundUser != null)
            throw new Exception("User already exists");

        await userRepository.AddAsync(new User()
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = hashPassword,
            Role = UserRole.User
        });
        
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<AuthSigninResponse> Signin(AuthSigninRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Password)) 
            throw new Exception("Email and password are required." );
        
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user == null)
            throw new Exception("Invalid email or password." );

        var authenticated = await identityService.VerifyPassword(
            request.Password, 
            user.PasswordHash);

        if (!authenticated)
            throw new Exception("Invalid email or password.");
        
        // Generate JWT token
        var accessToken = await identityService.GenerateAccessToken(user);
        var refreshToken = await  identityService.GenerateRefreshToken();

        if(string.IsNullOrWhiteSpace(accessToken) || 
           string.IsNullOrWhiteSpace(refreshToken))
            throw new Exception("Failed to create tokens.");
        
        var tokenHash = await identityService.HashToken(refreshToken);

        await refreshTokenRepository.AddAsync(CreateUserRefreshToken(tokenHash,user.Id));
        
        await unitOfWork.SaveChangesAsync();
        
        return new AuthSigninResponse(
            accessToken,
            refreshToken, 
            new ReadUserResponse(
                user.Id,
                user.Name,
                user.Email,
                 UserRole.User   ));
    }

    public async Task<User?> Me(Guid id)
    {
        return await userRepository.GetByIdAsync(id);
    }

    public async Task Signout(string token)
    {
        if(string.IsNullOrWhiteSpace(token))
            throw new Exception("Invalid refresh token.");
        
        var hashToken = await identityService.HashToken(token);
        
        var refreshToken  = await refreshTokenRepository
            .GetByHashAsync(hashToken);
        
        if (!ValidateRefreshToken(refreshToken))
           throw new Exception("Invalid refresh token.");

        refreshToken.RevokedAt = DateTime.UtcNow;

        await refreshTokenRepository.UpdateAsync(refreshToken);
        
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<AuthRefreshResponse> Refresh(string token)
    {
        if(string.IsNullOrWhiteSpace(token))
            throw new Exception("Invalid refresh token.");
        
        var hashToken = await identityService.HashToken(token);
        
        var refreshToken  = await refreshTokenRepository.
            GetByHashAsync(hashToken);
        
        if(refreshToken is null || !ValidateRefreshToken(refreshToken))
            throw new UnauthorizedAccessException("Invalid refresh token.");
        
        var user = await userRepository.GetByIdAsync(refreshToken.UserId);
        
        if (user is null)
            throw new UnauthorizedAccessException("Invalid refresh token.");
        
        var newRefreshToken = await identityService.GenerateRefreshToken();
        var newAccessToken = await identityService.GenerateAccessToken(user);
        var newHashToken = await identityService.HashToken(newRefreshToken);

        refreshToken.RevokedAt = DateTime.UtcNow;
        await refreshTokenRepository.UpdateAsync(refreshToken);
        
        await refreshTokenRepository.AddAsync(CreateUserRefreshToken(newHashToken,user.Id));
        
        await unitOfWork.SaveChangesAsync();
        
        return new AuthRefreshResponse(newAccessToken, newRefreshToken);
    }

    private RefreshToken CreateUserRefreshToken(string hashedToken, Guid userId)
    {
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            HashToken = hashedToken,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            RevokedAt = null
        }; 
    }
    private bool ValidateRefreshToken(RefreshToken token)
    {
        return token?.HashToken is not null
               && token.RevokedAt is null
               && token.ExpiresAt > DateTimeOffset.UtcNow;
    }
}