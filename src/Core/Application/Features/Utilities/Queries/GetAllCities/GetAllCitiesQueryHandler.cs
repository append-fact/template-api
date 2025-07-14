using Application.Common.Interfaces;
using Application.Common.Wrappers;
using AutoMapper;
using Domain.Entities.Common;
using MediatR;

namespace Application.Features.Utilities.Queries.GetAllCities
{
    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, PagedResponse<List<GetAllCitiesResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCitiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<GetAllCitiesResponse>>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            var listAllCities = await _unitOfWork.Repository<City>().ListAsync(new CitySpecification(request), cancellationToken);
            var totalRecords = await _unitOfWork.Repository<City>().CountAsync(new CitySpecification(request), cancellationToken);
            var result = _mapper.Map<List<GetAllCitiesResponse>>(listAllCities);

            return new PagedResponse<List<GetAllCitiesResponse>>(result, request.PageNumber, request.PageSize, totalRecords);
        }
    }
}
