using ClubManager.Domain.Entities;

namespace ClubManager.Domain.Events;

/// <summary>Raised when a new member is registered.</summary>
public record MemberRegisteredEvent(Member Member) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

/// <summary>Raised when a member is deactivated.</summary>
public record MemberDeactivatedEvent(Member Member) : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}
