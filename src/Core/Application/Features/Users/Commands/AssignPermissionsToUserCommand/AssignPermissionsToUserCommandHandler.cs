using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.AssignPermissionsToUserCommand
{
    public class AssignPermissionsToUserCommandHandler : IRequestHandler<AssignPermissionsToUserCommand, Response<string>>
    {
        private readonly IUserService _userService;

        public AssignPermissionsToUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Response<string>> Handle(AssignPermissionsToUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.AssignPermissionsToUserAsync(request.UserId, request.PermissionValues);
        }
    }
}
