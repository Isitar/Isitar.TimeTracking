namespace Isitar.TimeTracking.Infrastructure.Identity.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Token).IsRequired(true);
            builder.Property(x => x.JwtTokenId).IsRequired(true);
            builder.Property(x => x.Expires).IsRequired(true);
            builder.Property(x => x.Used).IsRequired(true);
            builder.Property(x => x.Invalidated).IsRequired(true);
            builder.HasOne(x => x.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}