using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.ForgotPasswordCommand
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Response<string>>
    {
        private readonly IAccountService _accountService;

        public ForgotPasswordCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<Response<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            await _accountService.ForgotPasswordAsync(request, "", cancellationToken);

            return new Response<string>(true, "Correo de restablecimiento enviado");
        }
    }
}
