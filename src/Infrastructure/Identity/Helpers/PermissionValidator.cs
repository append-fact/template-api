using Application.Common.Exceptions;
using Application.Features.Users.Commands.AssignPermissionsToUserCommand;
using Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Helpers
{
    public static class PermissionValidator
    {
        public static string GetValidPermissionName(string permissionName, List<Permission> definedPermissions)
        {
            if (string.IsNullOrWhiteSpace(permissionName))
                throw new ArgumentNullException(nameof(permissionName));

            if (!definedPermissions.Any(p => p.Name == permissionName))
            {
                throw new NotFoundException($"Permiso '{permissionName}' no válido.");
            }

            return permissionName;
        }
    }
}
