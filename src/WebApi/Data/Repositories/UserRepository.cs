using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Domain.Users;

namespace WebApi.Data.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<User?> AddAsync(User user)
    {
        context.Users.Add(user);
        
        await context.SaveChangesAsync();
        
        return user;
    }

    public async Task DeleteAsync(User user)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }
}
