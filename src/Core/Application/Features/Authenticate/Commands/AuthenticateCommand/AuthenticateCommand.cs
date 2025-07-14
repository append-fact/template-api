using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.AuthenticateCommand
{
    public class AuthenticateCommand : IRequest<Response<AuthenticationResponse>>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? IpAddress { get; set; }
    }

}