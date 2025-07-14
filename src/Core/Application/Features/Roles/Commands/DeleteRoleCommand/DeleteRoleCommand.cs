using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Roles.Commands.DeleteRoleCommand
{
    public class DeleteRoleCommand : IRequest<Response<string>>
    {
        public Guid RoleId { get; set; }
    }
}
