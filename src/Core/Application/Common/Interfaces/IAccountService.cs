using Application.Common.Wrappers;
using Application.Features.Authenticate.Commands.AuthenticateCommand;
using Application.Features.Authenticate.Commands.ForgotPasswordCommand;
using Application.Features.Authenticate.Commands.RegisterCommand;
using Application.Features.Authenticate.Commands.ResetPasswordCommand;
using System.Security.Claims;

namespace Application.Common.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterCommand request);
        Task<string> ConfirmEmailAsync(string userId, string code, CancellationToken cancellationToken);
        Task ForgotPasswordAsync(ForgotPasswordCommand request, string origin, CancellationToken cancellationToken);
        Task ResetPasswordAsync(ResetPasswordCommand request, CancellationToken cancellationToken);

        Task<Response<AuthenticationResponse>> RefreshTokenAsync(string refreshToken, string ipAddress);
        Task<Response<bool>> RevokeTokenAsync(string refreshToken, string ipAddress);

    }
}