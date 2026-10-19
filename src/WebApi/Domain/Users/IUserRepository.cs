namespace WebApi.Domain.Users;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task <User?> AddAsync(User user);
    void Update(User user);
    void Delete(User user);
}