using WebApi.Domain.Users;

namespace WebApi.Presentation.Users;

public record UpdateUserRequest(
    string Name,
    string Email,
    UserRole Role
    );