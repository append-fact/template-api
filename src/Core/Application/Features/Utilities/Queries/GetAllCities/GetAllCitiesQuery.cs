using Application.Common.Wrappers;
using MediatR;

namespace Application.Features.Utilities.Queries.GetAllCities
{
    public class GetAllCitiesQuery : PaginationCitiesParameters, IRequest<PagedResponse<List<GetAllCitiesResponse>>>
    {
    }
}
