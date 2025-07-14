using Application.Common.Wrappers;
using Application.DTOs.Common;
using MediatR;

namespace Application.Features.Utilities.Queries.GetAllProvinces
{
    public class GetAllProvincesQuery : IRequest<Response<List<ProvinceDTO>>>
    {
    }
}
