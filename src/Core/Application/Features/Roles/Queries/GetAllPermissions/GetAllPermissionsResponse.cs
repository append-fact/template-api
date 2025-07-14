using Application.DTOs;

namespace Application.Features.Roles.Queries.GetAllPermissions
{
    public class GetAllPermissionsResponse
    {
        public string Resource { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }
}
