using WebApi.Domain.Users;

namespace WebApi.Features.Users;

public class UserService(IUserRepository repository):IUserService
{
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await repository.GetAllAsync();
        return users;
    }
}