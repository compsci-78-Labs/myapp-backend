using WebApi.Domain.Users;

namespace WebApi.Features.Users;

public record CreateUserRequest(
    string Name,
    string Email,
    UserRole Role
    );