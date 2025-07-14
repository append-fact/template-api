using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Utilities.Commands.CreateContactCommand
{
    public class CreateContactCommand : IRequest<Response<string>>
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Origin { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
    }
}
