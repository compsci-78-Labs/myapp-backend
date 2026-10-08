using WebApi.Domain.Users;

namespace WebApi.Features.Users;

public record CreateUserRrequest(
    string Name,
    string Email,
    UserRole Role
    );