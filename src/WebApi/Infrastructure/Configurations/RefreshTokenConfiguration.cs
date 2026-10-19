using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Users;

namespace WebApi.Infrastructure.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.HashToken)
            .IsUnique();

        builder.Property(x => x.HashToken)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}