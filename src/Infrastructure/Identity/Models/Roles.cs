using System.Collections.ObjectModel;

namespace Identity.Models
{
    public static class Roles
    {
        public const string Admin = "Admin";

        public static IReadOnlyList<string> AllRoles { get; } = new ReadOnlyCollection<string>(new[]
        {
            Admin
        });

        public static bool IsDefault(string roleName) =>
            AllRoles.Contains(roleName);
    }
}