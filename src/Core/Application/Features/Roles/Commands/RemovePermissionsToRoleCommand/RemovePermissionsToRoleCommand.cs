using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Roles.Commands.RemovePermissionsToRoleCommand
{
    public class RemovePermissionsToRoleCommand : IRequest<Response<string>>
    {
        public Guid RoleId { get; set; }
        public List<string> PermissionValues { get; set; }
    }
}
