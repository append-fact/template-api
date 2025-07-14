using Domain.Common;

namespace Domain.Entities.Common
{
    public class Country : AuditableBaseEntity
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Province>? Provinces { get; set; }
    }
}
