namespace WebApi.Domain.Users;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByHashAsync(string hash);
    Task AddAsync(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
}