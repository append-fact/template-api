using Application.Common.Interfaces;
using Application.Common.Wrappers;
using Application.DTOs.Common;
using AutoMapper;
using Domain.Entities.Common;
using MediatR;

namespace Application.Features.Utilities.Queries.GetAllProvinces
{
    public class GetAllProvincesQueryHandler : IRequestHandler<GetAllProvincesQuery, Response<List<ProvinceDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProvincesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<List<ProvinceDTO>>> Handle(GetAllProvincesQuery request, CancellationToken cancellationToken)
        {
            var allProvinces = await _unitOfWork.Repository<Province>().ListAsync(cancellationToken);


            var response = _mapper.Map<List<ProvinceDTO>>(allProvinces);

            return new Response<List<ProvinceDTO>>(response);
        }
    }
}
