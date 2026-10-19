using WebApi.Domain.Users;

namespace WebApi.Presentation.Users;

public record CreateUserRequest(
    string Name,
    string Email,
    UserRole Role
    );