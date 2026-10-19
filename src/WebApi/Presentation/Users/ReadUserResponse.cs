using WebApi.Domain.Users;

namespace WebApi.Presentation.Users;

public record ReadUserResponse(
    Guid Id,
    string Name,
    string Email,
    UserRole Role);