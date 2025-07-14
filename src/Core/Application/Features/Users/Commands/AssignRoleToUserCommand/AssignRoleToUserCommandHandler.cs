using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.AssignRoleToUserCommand
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, Response<string>>
    {
        private readonly IUserService _userService;

        public AssignRoleToUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Response<string>> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.AssignRoleToUserAsync(request.UserId, request.RoleId);
        }
    }

}
