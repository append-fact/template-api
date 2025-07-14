using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Roles.Commands.AssignPermissionsToRoleCommand
{
    public class AssignPermissionsToRoleCommandHandler : IRequestHandler<AssignPermissionsToRoleCommand, Response<string>>
    {
        private readonly IRolesService _rolesService;

        public AssignPermissionsToRoleCommandHandler(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<Response<string>> Handle(AssignPermissionsToRoleCommand request, CancellationToken cancellationToken)
        {
            return await _rolesService.AssignPermissionsToRoleAsync(request.RoleId, request.PermissionValues);
        }
    }
}
