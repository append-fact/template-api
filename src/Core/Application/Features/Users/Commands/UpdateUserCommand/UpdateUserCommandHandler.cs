using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUserCommand
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<string>>
    {
        private readonly IUserService _userService;

        public UpdateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Response<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserAsync(request, request.Id);

            return new Response<string>(true, "Usuario actualizado correctamente");
        }
    }
}
