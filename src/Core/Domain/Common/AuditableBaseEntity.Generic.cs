using Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public abstract class AuditableBaseEntity<T>: Entity<T>, IAuditableBaseEntity
    {
        [Required, MaxLength(50)]
        public string? CreatedBy { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [MaxLength(50)]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [MaxLength(50)]
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}