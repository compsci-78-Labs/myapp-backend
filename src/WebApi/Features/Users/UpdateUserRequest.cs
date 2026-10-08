using WebApi.Domain.Users;

namespace WebApi.Features.Users;

public record UpdateUserRequest(
    string Name,
    string Email,
    UserRole Role
    );