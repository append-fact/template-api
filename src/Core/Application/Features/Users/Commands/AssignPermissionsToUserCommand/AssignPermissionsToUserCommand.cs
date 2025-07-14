using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Users.Commands.AssignPermissionsToUserCommand
{
    public class AssignPermissionsToUserCommand : IRequest<Response<string>>
    {
        public Guid UserId { get; set; }
        public List<string> PermissionValues { get; set; }
    }
}
