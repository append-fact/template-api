namespace Domain.Common.Interfaces
{
    public interface IEntityWithEvents
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
