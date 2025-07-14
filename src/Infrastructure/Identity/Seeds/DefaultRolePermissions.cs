using Identity.Context;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.Seeds
{
    public static class DefaultRolePermissions
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, IdentityContext context)
        {
            if (!context.RoleClaims.Any())
            {
                foreach (var roleName in Roles.AllRoles)
                {
                    var role = await roleManager.FindByNameAsync(roleName);
                    if (role == null) continue;

                    var existingClaims = await roleManager.GetClaimsAsync(role);


                    var permissionsToAssign = GetPermissionsForRole(roleName);

                    foreach (var permission in permissionsToAssign)
                    {

                        if (!existingClaims.Any(c => c.Type == Claims.Permission && c.Value == permission.Name))
                        {

                            await roleManager.AddClaimAsync(role, new Claim(Claims.Permission, permission.Name));
                        }
                    }
                }
            }
        }

        private static List<Permission> GetPermissionsForRole(string roleName)
        {
            return roleName switch
            {
                nameof(Roles.Admin) => Permissions.All.ToList(),
                _ => new List<Permission>() 
            };
        }
    }
}
