using Domain.Common.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Common
{
    public class Entity<TKey> : IEntityWithEvents
    {
        [JsonIgnore]
        private readonly List<DomainEvent> _domainEvents = new();

        public TKey Id { get; set; }

        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents
            => _domainEvents.AsReadOnly();

        public void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void RemoveDomainEvent(DomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();

        IReadOnlyCollection<DomainEvent> IEntityWithEvents.DomainEvents
            => DomainEvents;
        void IEntityWithEvents.ClearDomainEvents() => ClearDomainEvents();



    }
}
