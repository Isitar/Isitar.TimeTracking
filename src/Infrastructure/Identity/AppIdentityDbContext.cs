namespace Isitar.TimeTracking.Infrastructure.Identity
{
    using System;
    using Application.Common.Enums;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }


        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppIdentityDbContext).Assembly);
            builder.Entity<AppRole>()
                .HasData(new[]
                {
                    new AppRole
                    {
                        Id = Guid.Parse("79690AED-86B2-4480-A726-9FE5F0D8B35B"),
                        Name = RoleNames.Admin,
                        ConcurrencyStamp = "79690AED-86B2-4480-A726-9FE5F0D8B35B",
                        NormalizedName = RoleNames.Admin.ToUpper(),
                    }
                });
        }
        
    }
}