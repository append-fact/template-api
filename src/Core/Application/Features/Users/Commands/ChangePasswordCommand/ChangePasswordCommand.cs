using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Users.Commands.ChangePasswordCommand
{
    public class ChangePasswordCommand : IRequest<Response<string>>
    {
        public string Password { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmNewPassword { get; set; } = default!;
    }
}
