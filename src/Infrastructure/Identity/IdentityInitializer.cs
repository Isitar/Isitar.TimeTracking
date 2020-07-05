namespace Isitar.TimeTracking.Infrastructure.Identity
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Application.Common.Enums;
    using Application.Common.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class IdentityInitializer : IIdentityInitializer
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;

        public IdentityInitializer(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task Initialize()
        {
            if (roleManager.Roles.Any())
            {
                return;
            }

            var adminId = Guid.NewGuid();
            await roleManager.CreateAsync(new AppRole {Id = adminId, Name = RoleNames.Admin, ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = RoleNames.Admin.ToUpper()});
            var adminRole = await roleManager.Roles.FirstAsync(r => r.Id.Equals(adminId));
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.PermissionClaimType, Permissions.Admin));
        }
    }
}