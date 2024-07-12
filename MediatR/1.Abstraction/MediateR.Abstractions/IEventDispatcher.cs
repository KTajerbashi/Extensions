using MediatR;

namespace MediateR.Abstractions;

public interface IEventDispatcher
{
    Task PublishDomainEventAsync<TDomainEvent>(TDomainEvent @event) 
        where TDomainEvent : class, IDomainEvent;

}
public interface IDomainEvent : INotification
{
}


public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : class, IDomainEvent
{

}
