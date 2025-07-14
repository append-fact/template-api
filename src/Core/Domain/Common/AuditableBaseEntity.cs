namespace Domain.Common
{
    public abstract class AuditableBaseEntity : AuditableBaseEntity<Guid>
    {
        protected AuditableBaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
