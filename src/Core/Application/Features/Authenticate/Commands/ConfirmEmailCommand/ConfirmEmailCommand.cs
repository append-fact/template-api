using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.ConfirmEmailCommand
{
    public class ConfirmEmailCommand : IRequest<Response<string>>
    {
        public Guid Id { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
