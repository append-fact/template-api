using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.RevokeRefreshTokenCommand
{
    public class RevokeRefreshtokenCommand : IRequest<Response<bool>>
    {

        public string? RefreshToken { get; set; }
        public string? IpAddress { get; set; }
    }
}
