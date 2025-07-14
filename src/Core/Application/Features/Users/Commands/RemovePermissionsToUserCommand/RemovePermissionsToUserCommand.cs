using Application.Common.Wrappers;
using Application.Features.Users.Commands.AssignPermissionsToUserCommand;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.RemovePermissionsToUserCommand
{
    public class RemovePermissionsToUserCommand : IRequest<Response<string>>
    {
        public Guid UserId { get; set; }
        public List<string> PermissionValues { get; set; } 
    }
}
