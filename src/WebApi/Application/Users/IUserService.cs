using WebApi.Domain.Users;

namespace WebApi.Application.Users;


public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task <User?> AddAsync(User user);
    Task <User?> Update(User user);
    Task Delete(Guid id);
}