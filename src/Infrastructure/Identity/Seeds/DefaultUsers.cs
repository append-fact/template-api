using Identity.Context;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IdentityContext context)
        {
            if (!context.Users.Any())
            {
                var superAdminUser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "administracion@append.com.ar",
                    FirstName = "Super",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                if (userManager.Users.All(u => u.Id != superAdminUser.Id))
                {
                    var user = await userManager.FindByEmailAsync(superAdminUser.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(superAdminUser, "P4ssw0rd!");
                        await userManager.AddToRoleAsync(superAdminUser, Roles.Admin.ToString());
                    }
                }

                
            }
        }
    }
}
