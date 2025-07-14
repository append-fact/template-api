using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUserCommand
{
    public class DeleteUserCommand : IRequest<Response<string>>
    {
        public Guid Id { get; set; }
    }
}
