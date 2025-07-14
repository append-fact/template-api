using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Roles.Commands.CreateRoleCommand
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Response<string>>
    {
        private readonly IRolesService _rolesService;

        public CreateRoleCommandHandler(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<Response<string>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            return await _rolesService.CreateRoleAsync(request.RoleName);
        }
    }
}
