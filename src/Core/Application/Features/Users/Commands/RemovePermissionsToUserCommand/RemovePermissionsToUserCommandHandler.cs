using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.RemovePermissionsToUserCommand
{
    public class RemovePermissionsToUserCommandHandler : IRequestHandler<RemovePermissionsToUserCommand, Response<string>>
    {
        private readonly IUserService _userService;

        public RemovePermissionsToUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Response<string>> Handle(RemovePermissionsToUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.RemovePermissionsToUserAsync(request.UserId, request.PermissionValues);
        }
    }
}
