using Application.Common.Wrappers;
using Application.DTOs;
using Application.Features.Roles.Queries.GetAllPermissions;

namespace Application.Common.Interfaces
{
    public interface IRolesService
    {
        Task<Response<string>> CreateRoleAsync(string roleName);
        Task<Response<string>> UpdateRoleAsync(Guid roleId, string roleName);
        Task<Response<string>> DeleteRoleAsync(Guid roleId);
        Task<Response<string>> AssignPermissionsToRoleAsync(Guid roleId, List<string> permissionValues);

        Task<Response<List<GetAllPermissionsResponse>>> GetAllPermissionsAsync();
        Task<Response<List<RolesWithPermissionsResponseDTO>>> GetAllRolesWithPermissionsAsync();
        Task<bool> UserHasPermissionAsync(string permission);


        Task<Response<string>> RemovePermissionsToRoleAsync(Guid roleId, List<string> permissionValues);
    }
}
