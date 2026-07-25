namespace WebApi.Domain.Users;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User product);
    Task DeleteAsync(User product);
}