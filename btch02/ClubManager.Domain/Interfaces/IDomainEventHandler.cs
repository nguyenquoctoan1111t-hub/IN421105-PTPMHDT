using ClubManager.Domain.Events;

namespace ClubManager.Domain.Interfaces;

/// <summary>
/// Observer Pattern: event handler interface.
/// ISP: each handler only subscribes to events it cares about.
/// </summary>
public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent);
}
