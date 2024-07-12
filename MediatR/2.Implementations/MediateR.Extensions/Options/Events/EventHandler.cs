using MediateR.Abstractions;

namespace MediateR.Extensions.Options.Events;

public abstract class EventHandler<TEvent> : IEventHandler<TEvent>
    where TEvent : class, IDomainEvent
{
    public abstract Task Handle(TEvent notification, CancellationToken cancellationToken);
}
