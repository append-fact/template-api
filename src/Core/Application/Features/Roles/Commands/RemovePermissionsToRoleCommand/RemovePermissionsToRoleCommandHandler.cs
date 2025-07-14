using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Roles.Commands.RemovePermissionsToRoleCommand
{
    public class RemovePermissionsToRoleCommandHandler : IRequestHandler<RemovePermissionsToRoleCommand, Response<string>>
    {
        private readonly IRolesService _rolesService;

        public RemovePermissionsToRoleCommandHandler(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<Response<string>> Handle(RemovePermissionsToRoleCommand request, CancellationToken cancellationToken)
        {
            return await _rolesService.RemovePermissionsToRoleAsync(request.RoleId, request.PermissionValues);
        }
    }
}
