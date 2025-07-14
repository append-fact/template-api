using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Roles.Commands.CreateRoleCommand
{
    public class CreateRoleCommand : IRequest<Response<string>>
    {
        public string RoleName { get; set; }
    }
}
