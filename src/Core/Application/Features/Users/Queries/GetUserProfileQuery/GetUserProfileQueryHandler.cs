using Application.Common.Interfaces;
using Application.Common.Wrappers;
using Application.Features.Users.Queries.GetUserProfileQuery;
using AutoMapper;
using MediatR;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Response<GetUserProfileResponse>>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public GetUserProfileQueryHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<Response<GetUserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await _userService.GetUser(null, cancellationToken);

        return new Response<GetUserProfileResponse>(currentUser.Data);
    }
}
