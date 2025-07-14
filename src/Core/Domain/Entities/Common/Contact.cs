using Domain.Common;

namespace Domain.Entities.Common
{
    public class Contact : AuditableBaseEntity
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Origin { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
    }
}
