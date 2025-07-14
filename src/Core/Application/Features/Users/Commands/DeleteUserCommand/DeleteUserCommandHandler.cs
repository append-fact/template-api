using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUserCommand
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response<string>>
    {
        private readonly IUserService _userService;

        public DeleteUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(request.Id.ToString());

            return new Response<string>(true, "Usuario eliminado correctamente");
        }
    }
}
