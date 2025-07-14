using Domain.Common;

namespace Domain.Entities.Common
{
    public class Province : AuditableBaseEntity
    {
        public string? Name { get; set; }
        public Guid? CountryId { get; set; }

        public virtual Country? Country { get; set; }
        public virtual ICollection<City>? Cities { get; set; }
    }
}
