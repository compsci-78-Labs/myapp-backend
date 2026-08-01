using Microsoft.EntityFrameworkCore;
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
        await context.Users.AddAsync(user);

        return user;
    }

    public void Update(User user)
    {
      context.Users.Update(user);
    }

    public void Delete(User user)
    {
         context.Users.Remove(user);
    }
}
