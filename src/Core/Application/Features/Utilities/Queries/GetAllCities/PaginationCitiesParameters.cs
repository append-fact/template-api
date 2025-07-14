using Application.Common.Parameters;

namespace Application.Features.Utilities.Queries.GetAllCities
{
    public class PaginationCitiesParameters : RequestParameters
    {
        public Guid? Id { get; set; }
        public Guid? ProvinceId { get; set; }
        public string? Name { get; set; }
    }
}
