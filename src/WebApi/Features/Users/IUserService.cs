using WebApi.Domain.Users;
namespace WebApi.Features.Users;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task <User?> AddAsync(User user);
    Task <User?> Update(User user);
    Task <User?> Delete(User user);
}