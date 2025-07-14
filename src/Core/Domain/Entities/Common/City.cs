using Domain.Common;

namespace Domain.Entities.Common
{
    public class City : AuditableBaseEntity
    {
        public string? Name { get; set; }
        public Guid ProvinceId { get; set; }

        public virtual Province? Province { get; set; }
    }
}
