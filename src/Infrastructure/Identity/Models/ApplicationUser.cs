using Domain.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        [JsonIgnore]
        private readonly List<DomainEvent> _domainEvents = new();

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AreaNumber { get; set; }
        public Uri? UrlImage { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void RemoveDomainEvent(DomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
