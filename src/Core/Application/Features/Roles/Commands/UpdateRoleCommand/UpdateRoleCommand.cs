using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Roles.Commands.UpdateRoleCommand
{
    public class UpdateRoleCommand : IRequest<Response<string>>
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
