using WebApi.Domain.Users;
namespace WebApi.Features.Users;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
}