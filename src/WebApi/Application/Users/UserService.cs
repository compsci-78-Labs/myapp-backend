using WebApi.Infrastructure;
using WebApi.Domain.Users;

namespace WebApi.Application.Users;

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
    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await repository.GetByEmailAsync(email);
        return user;
    }
    public async Task<User?> AddAsync(User user)
    {
        var foundUser = await repository.GetByEmailAsync(user.Email);
        
        if (foundUser is not null)
            throw new Exception("User already exists");
        
        await repository.AddAsync(user);
        
        await unitOfWork.SaveChangesAsync();
        
        return user;
    }

    public async Task<User?> Update(User user)
    {
        var userById = await repository.GetByIdAsync(user.Id);
        var userByEmail = await repository.GetByEmailAsync(user.Email);

        if (userById is null) 
            throw new Exception("User not found") ;

        if ( userByEmail is not null  && 
             userByEmail?.Id != userById.Id ) 
            throw new Exception("Email is already in use.") ;

        
        userById.Name = user.Name;
        userById.Email = user.Email;
        userById.Role = user.Role;
        
        repository.Update(userById);
        
        await unitOfWork.SaveChangesAsync();
        
        return userById;
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