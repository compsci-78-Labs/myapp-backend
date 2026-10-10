using WebApi.Data;
using WebApi.Domain.Users;

namespace WebApi.Features.Users;

public class UserService(IUserRepository repository,IUnitOfWork unitOfWork):IUserService
{
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await repository.GetAllAsync();
        return users;
    }
    public async Task<User?> GetByIdAsync(Guid id)
    {
        var user = await repository.GetByIdAsync(id);
        return user;
    }

    public async Task<User?> AddAsync(User user)
    {
        await repository.AddAsync(user);
        
        await unitOfWork.SaveChangesAsync();
        
        return user;
    }

    public async Task<User?> Update(User user)
    {
        var foundUser = await repository.GetByIdAsync(user.Id);
        
        if (foundUser == null) 
            throw new Exception("User not found") ;
        
        foundUser.Name = user.Name;
        foundUser.Email = user.Email;
        foundUser.Role = user.Role;
        
        repository.Update(foundUser);
        
        await unitOfWork.SaveChangesAsync();
        
        return foundUser;
    }

    public async Task Delete(Guid id)
    {
        var foundUser = await repository.GetByIdAsync(id);
        
        if (foundUser == null) 
            throw new Exception("User not found") ;

        repository.Delete(foundUser);
        
        await unitOfWork.SaveChangesAsync();
    }
}