using Application.Common.Interfaces;
using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.ConfirmEmailCommand
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Response<string>>
    {
        private readonly IAccountService _accountService;

        public ConfirmEmailCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<Response<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var res = await _accountService.ConfirmEmailAsync(request.Id.ToString(), request.Token, cancellationToken);

            return new Response<string>(true, res);
        }
    }
}
