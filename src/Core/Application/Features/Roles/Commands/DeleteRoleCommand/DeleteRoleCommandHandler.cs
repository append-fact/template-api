using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Roles.Commands.DeleteRoleCommand
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Response<string>>
    {
        private readonly IRolesService _rolesService;

        public DeleteRoleCommandHandler(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<Response<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            return await _rolesService.DeleteRoleAsync(request.RoleId);
        }
    }
}
