using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Roles.Commands.AssignPermissionsToRoleCommand
{
    public class AssignPermissionsToRoleCommand : IRequest<Response<string>>
    {
        public Guid RoleId { get; set; }
        public List<string> PermissionValues { get; set; } = new();
    }
}
