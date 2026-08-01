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

    public async Task<User?> Update(User userUpdates)
    {
        var userDb = await repository.GetByIdAsync(userUpdates.Id);
        
        if (userDb == null) 
            throw new Exception("User not found") ;
        
        userDb.Name = userUpdates.Name;
        userDb.Email = userUpdates.Email;
        
        await unitOfWork.SaveChangesAsync();
        
        return userDb;
    }

    public async Task<User?> Delete(User user)
    {
        repository.Delete(user);
        
        await unitOfWork.SaveChangesAsync();
        
        return user;
    }
}