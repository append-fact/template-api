using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.RemoveRoleToUserCommand
{
    public class RemoveRoleToUserCommandHandler : IRequestHandler<RemoveRoleToUserCommand, Response<string>>
    {
        private readonly IUserService _userService;

        public RemoveRoleToUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Response<string>> Handle(RemoveRoleToUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.RemoveRoleToUserAsync(request.UserId, request.RoleId);
        }
    }
}
