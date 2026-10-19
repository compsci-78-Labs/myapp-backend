namespace WebApi.Domain.Users;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string HashToken { get; set; } = null;
    public Guid UserId { get; set; }
    public User User { get; set; } = null;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
};