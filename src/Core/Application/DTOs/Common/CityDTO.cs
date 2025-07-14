namespace Application.DTOs.Common
{

    public class CityDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public ProvinceDTO? Province { get; set; }
    }
}
