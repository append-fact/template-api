using Application.Common.Parameters;
using Ardalis.Specification;
using Domain.Entities.Common;

namespace Application.Features.Utilities.Queries.GetAllCities
{
    public class CitySpecification : Specification<City>
    {
        public CitySpecification(PaginationCitiesParameters parameters)
        {
            Query.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                 .Take(parameters.PageSize);

            if (parameters.Id.HasValue)
            {
                Query.Where(c => c.Id == parameters.Id);
            }
            if (!string.IsNullOrWhiteSpace(parameters.Name))
            {
                Query.Where(c => c.Name.Contains(parameters.Name));
            }

            if (parameters.ProvinceId.HasValue)
            {
                Query.Where(c => c.Province.Id == parameters.ProvinceId);
            }

            Query.Include(c => c.Province)
                    .ThenInclude(c => c!.Country);

            Query.ApplyOrdering(parameters.SortColumn, parameters.SortOrder);
        }
    }
}
