using Application.DTOs.Common;

namespace Application.Features.Utilities.Queries.GetAllCities
{
    public class GetAllCitiesResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public ProvinceDTO? Province { get; set; }
    }
}
