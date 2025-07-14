using Application.Common.Interfaces;
using Application.Common.Wrappers;
using Application.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Features.Users.Queries.GetUserByIdQuery
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Response<UserDTO>>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<Response<UserDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _userService.GetUser(request.Id.ToString(), cancellationToken);

            var response = _mapper.Map<UserDTO>(currentUser.Data);

            return new Response<UserDTO>(response);
        }
    }
}
