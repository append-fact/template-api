using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Roles.Commands.UpdateRoleCommand
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Response<string>>
    {
        private readonly IRolesService _rolesService;

        
        public UpdateRoleCommandHandler(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<Response<string>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            
            return await _rolesService.UpdateRoleAsync(request.RoleId, request.RoleName);
        }
    }
}
