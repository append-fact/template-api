using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.ResetPasswordCommand
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Response<string>>
    {
        private readonly IAccountService _accountService;

        public ResetPasswordCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            await _accountService.ResetPasswordAsync(request, cancellationToken);

            return new Response<string>(true, "La contraseña se reestableció correctamente");
        }
    }
}
