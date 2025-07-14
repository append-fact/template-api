using Application.Common.Wrappers;
using Application.DTOs;
using MediatR;

namespace Application.Features.Users.Queries.GetUserByIdQuery
{
    public class GetUserByIdQuery : IRequest<Response<UserDTO>>
    {
        public Guid Id { get; set; }
    }
}
