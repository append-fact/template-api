namespace Domain.Common.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAndClearEvents(IEnumerable<IEntityWithEvents> entitiesWithEvents);
    }
}