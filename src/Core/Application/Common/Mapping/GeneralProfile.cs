using Application.DTOs;
using Application.DTOs.Common;
using Application.Features.Authenticate.Commands.AuthenticateCommand;
using Application.Features.Utilities.Commands.CreateContactCommand;
using Application.Features.Utilities.Queries.GetAllCities;
using AutoMapper;
using Domain.Entities.Common;
using Domain.Entities.ValueObjects;

namespace Application.Common.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {

            #region Commands

            CreateMap<CreateContactCommand, Contact>().ReverseMap();

            #endregion

            #region Queries

            CreateMap<GetAllCitiesResponse, City>().ReverseMap();
            
            #endregion

            #region DTOs

            CreateMap<UserDTO, AuthenticationResponse>().ReverseMap();
            CreateMap<AddressDTO, Address>().ReverseMap();
            CreateMap<PhoneDTO, Phone>().ReverseMap();
            CreateMap<CityDTO, City>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<ProvinceDTO, Province>().ReverseMap();
           
            #endregion
        }
    }
}
