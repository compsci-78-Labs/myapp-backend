namespace WebApi.Domain.Users;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task <User?> AddAsync(User user);
    Task DeleteAsync(User user);
}