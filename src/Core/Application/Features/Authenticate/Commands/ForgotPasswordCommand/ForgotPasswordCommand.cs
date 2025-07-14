using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.ForgotPasswordCommand
{
    public class ForgotPasswordCommand : IRequest<Response<string>>
    {
        public string Email { get; set; } = default!;
    }
}
