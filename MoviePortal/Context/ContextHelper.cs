using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviePortal.Context
{
    public static class ContextHelper
    {
        public static async Task Seeding(MoviePortalContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Where(p=> p.NormalizedName.Equals("Admin")).Any())
            {
                var adminRole = new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                };

                await roleManager.CreateAsync(adminRole);
            }

            if (!userManager.Users.Where(p=> p.UserName.Equals("Admin")).Any())
            {
                var adminUser = new IdentityUser
                {
                    UserName = "Admin",
                    Email = "Admin@gmail.com"
                };
                var result = await userManager.CreateAsync(adminUser, "password");

                if (result.Succeeded)
                {
                    var role = await roleManager.FindByNameAsync("Admin");

                    await userManager.AddToRoleAsync(await userManager.FindByNameAsync("Admin"), role.Name);
                }
            }
        }
    }
}
