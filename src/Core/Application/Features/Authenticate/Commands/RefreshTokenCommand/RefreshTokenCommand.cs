using Application.Common.Wrappers;
using Application.Features.Authenticate.Commands.AuthenticateCommand;
using MediatR;

namespace Application.Features.Authenticate.Commands.RefreshTokenCommand
{
    public class RefreshTokenCommand : IRequest<Response<AuthenticationResponse>>
    {
        public string? RefreshToken { get; set; }
        public string? IpAddress { get; set; }
    }

    
}
