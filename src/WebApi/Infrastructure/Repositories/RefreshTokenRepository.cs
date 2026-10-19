using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Users;

namespace WebApi.Infrastructure.Repositories;

public class RefreshTokenRepository(AppDbContext db) : IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByHashAsync(string hash)
        => db.RefreshTokens
            .SingleOrDefaultAsync(x => x.HashToken == hash);

    public async Task AddAsync(RefreshToken refreshToken)
        => await db.RefreshTokens.AddAsync(refreshToken);

    public Task UpdateAsync(RefreshToken refreshToken)
    {
        db.RefreshTokens.Update(refreshToken);
        return Task.CompletedTask;
    }
}