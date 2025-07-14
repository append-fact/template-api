using Application.Common.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.RemoveRoleToUserCommand
{
    public class RemoveRoleToUserCommand : IRequest<Response<string>>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
