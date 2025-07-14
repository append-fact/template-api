using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUsersQuery
{
    public class GetAllUsersQuery : PaginationUserParameters, IRequest<PagedResponse<List<GetAllUsersResponse>>>
    {
    }
}
