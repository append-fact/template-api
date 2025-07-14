using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Wrappers;
using Application.DTOs;
using Application.Features.Roles.Queries.GetAllPermissions;
using Identity.Context;
using Identity.Helpers;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Identity.Services
{
    internal sealed class RolesService : IRolesService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IdentityContext _identityContext;
        private readonly CurrentUser _currentUser;

        public RolesService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IdentityContext identityContext,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _identityContext = identityContext;
            _currentUser = currentUserService.User;
        }

        public async Task<Response<string>> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return new Response<string>(false, "El nombre del rol es requerido.");

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
                return new Response<string>(false, "El rol ya existe.");

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!result.Succeeded)
                return new Response<string>(false, "Error al crear el rol.");

            return new Response<string>($"Rol {roleName} creado correctamente.");
        }
        public async Task<Response<string>> UpdateRoleAsync(Guid roleId, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return new Response<string>(false, "El rol no existe.");
            }

            role.Name = roleName;

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return new Response<string>(false, "Error al actualizar el rol.");
            }

            return new Response<string>($"Rol {roleName} actualizado correctamente.");
        }
        public async Task<Response<string>> DeleteRoleAsync(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                throw new ApiException("El rol no existe.");
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            if (usersInRole.Any())
            {
                throw new ApiException("El rol no puede ser eliminado porque tiene usuarios asignados.");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                throw new ApiException("No se pudo eliminar el rol.");
            }

            return new Response<string>("Rol eliminado correctamente.");
        }

        public async Task<Response<string>> AssignPermissionsToRoleAsync(Guid roleId, List<string> permissionValues)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
                throw new ApiException("El rol no existe.");

            var allPermissions = Permissions.All.Select(p => p.Name).ToHashSet(); // Lista de permisos definidos.

            var invalidPermissions = permissionValues
                .Where(p => !PermissionValidator.GetValidPermissionName(p, Permissions.All.ToList()).Any()) // Se valida si el permiso es válido.
                .ToList();

            if (invalidPermissions.Any())
                throw new ApiException($"Los siguientes permisos no son válidos: {string.Join(", ", invalidPermissions)}");

            var existingClaims = await _roleManager.GetClaimsAsync(role);
            var existingPermissionValues = existingClaims
                .Where(c => c.Type == Claims.Permission)
                .Select(c => c.Value)
                .ToHashSet();

            var newPermissions = permissionValues
                .Where(p => !existingPermissionValues.Contains(p))
                .ToList();

            foreach (var permission in newPermissions)
            {
                var result = await _roleManager.AddClaimAsync(role, new Claim(Claims.Permission, permission));
                if (!result.Succeeded)
                    throw new ApiException($"No se pudo asignar el permiso {permission} al rol.");
            }

            return new Response<string>($"{newPermissions.Count} permiso(s) asignado(s) correctamente al rol.");
        }







        public async Task<Response<string>> RemovePermissionsToRoleAsync(Guid roleId, List<string> permissionValues)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
                throw new ApiException("El rol no existe.");

            var existingClaims = await _roleManager.GetClaimsAsync(role);

            var removedCount = 0;

            foreach (var permissionValue in permissionValues)
            {
                PermissionValidator.GetValidPermissionName(permissionValue, Permissions.All.ToList());

                var claimToRemove = existingClaims.FirstOrDefault(c => c.Type == Claims.Permission && c.Value == permissionValue);
                if (claimToRemove != null)
                {
                    var result = await _roleManager.RemoveClaimAsync(role, claimToRemove);
                    if (!result.Succeeded)
                        throw new ApiException($"No se pudo remover el permiso '{permissionValue}' del rol.");

                    removedCount++;
                }
            }

            return new Response<string>($"Se revocaron {removedCount} permisos del rol.");
        }





        public async Task<Response<List<GetAllPermissionsResponse>>> GetAllPermissionsAsync()
        {
            var allPermissions = Permissions.All;

            var result = Permissions.All
                .GroupBy(p => p.Resource)
                .Select(g => new GetAllPermissionsResponse
                {
                    Resource = g.Key,
                    Permissions = g.Select(p => new PermissionDTO
                    {
                        Name = p.Description,
                        Action = p.Action,
                        Value = p.Name
                    }).ToList()
                }).ToList();

            return new Response<List<GetAllPermissionsResponse>>(result);
        }


        public async Task<Response<List<RolesWithPermissionsResponseDTO>>> GetAllRolesWithPermissionsAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var result = new List<RolesWithPermissionsResponseDTO>();

            foreach (var role in roles)
            {
                var permissions = await _identityContext.RoleClaims
                    .Where(rc => rc.RoleId == role.Id && rc.ClaimType == "Permission")
                    .Select(rc => new PermissionDTO
                    {
                        Value = rc.ClaimValue
                    })
                    .ToListAsync();

                result.Add(new RolesWithPermissionsResponseDTO
                {
                    RoleId = Guid.Parse(role.Id),
                    RoleName = role.Name ?? string.Empty,
                    Permissions = permissions
                });
            }

            return new Response<List<RolesWithPermissionsResponseDTO>>(result);
        }

        public async Task<bool> UserHasPermissionAsync(string permission)
        {
            var user = await _userManager.FindByIdAsync(_currentUser.Id);
            if (user == null) return false;

            // Verificar permisos asignados por rol.
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null) continue;

                var roleClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var roleClaim in roleClaims)
                {
                    if (roleClaim.Type == Claims.Permission &&
                        roleClaim.Value.Equals(permission, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            // Verificar permisos directos asignados al usuario en la tabla AspNetUserClaims.
            var userClaims = await _userManager.GetClaimsAsync(user);
            foreach (var userClaim in userClaims)
            {
                if (userClaim.Type == Claims.Permission &&
                    userClaim.Value.Equals(permission, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }


    }
}
