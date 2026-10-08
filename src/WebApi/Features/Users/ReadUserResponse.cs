using WebApi.Domain.Users;

namespace WebApi.Features.Users;

public record ReadUserResponse(
    Guid Id,
    string Name,
    string Email,
    UserRole Role);