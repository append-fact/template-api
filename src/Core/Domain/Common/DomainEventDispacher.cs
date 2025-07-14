using Domain.Common.Interfaces;
using MediatR;

namespace Domain.Common
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task DispatchAndClearEvents(IEnumerable<IEntityWithEvents> entitiesWithEvents)
        {
            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToArray();

                entity.ClearDomainEvents();

                foreach (var @event in events)
                    await _mediator.Publish(@event).ConfigureAwait(false);
            }
        }
    }
}