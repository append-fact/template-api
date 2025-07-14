using System.Collections.ObjectModel;

namespace Identity.Models
{
    public static class Permissions
    {
        private static readonly Permission[] AllPermissions =
        [
            // Roles
            new("Create Roles", Actions.Create, Resources.Roles, Roles.Admin),
            new("Search Roles", Actions.Search, Resources.Roles, Roles.Admin),
            new("Update Roles", Actions.Update, Resources.Roles, Roles.Admin),
            new("Delete Roles", Actions.Delete, Resources.Roles, Roles.Admin),
            new("View Roles", Actions.View, Resources.Roles, Roles.Admin),

            // RolePermissions
            new("View RoleClaims", Actions.View, Resources.RoleClaims, Roles.Admin),
            new("Search RoleClaims", Actions.Search, Resources.RoleClaims, Roles.Admin),
            new("Update RoleClaims", Actions.Update, Resources.RoleClaims, Roles.Admin),
            new("Delete RoleClaims", Actions.Delete, Resources.RoleClaims, Roles.Admin),

            // Users
            new("View User Profile", Actions.View, Resources.Users, Roles.Admin),
            new("Search Users", Actions.Search, Resources.Users, Roles.Admin),
            new("Update User Profile", Actions.Update, Resources.Users, Roles.Admin),
            new("Delete User", Actions.Delete, Resources.Users),

            // UserRoles
            new("View UserRoles", Actions.View, Resources.UserRoles, Roles.Admin),
            new("Search UserRoles", Actions.Search, Resources.UserRoles, Roles.Admin),
            new("Update UserRoles", Actions.Update, Resources.UserRoles, Roles.Admin),
            new("Update UserRoles", Actions.Delete, Resources.UserRoles, Roles.Admin),

            // UserPermissions
            new("View UserPermissions", Actions.View, Resources.UserPermissions, Roles.Admin),
            new("Search UserPermissions", Actions.Search, Resources.UserPermissions, Roles.Admin),
            new("Update UserPermissions", Actions.Update, Resources.UserPermissions, Roles.Admin),
            new("Delete UserPermissions", Actions.Delete, Resources.UserPermissions, Roles.Admin),
        ];

        public static IReadOnlyList<Permission> All { get; } = new ReadOnlyCollection<Permission>(AllPermissions);

    }

    public record Permission(string Description, string Action, string Resource, params string[] AllowedRoles)
    {
        public string Name => NameFor(Action, Resource);
        public static string NameFor(string action, string resource)
        {
            return $"{resource}.{action}";
        }
    }
}
