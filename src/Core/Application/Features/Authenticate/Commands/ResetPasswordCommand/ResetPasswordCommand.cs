using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.ResetPasswordCommand
{
    public class ResetPasswordCommand : IRequest<Response<string>>
    {
        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string Token { get; set; } = default!;
    }
}
