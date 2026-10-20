using ClubManager.Domain.Events;
using ClubManager.Domain.Interfaces;

namespace ClubManager.Infrastructure.EventHandlers;

/// <summary>
/// Observer Pattern: reacts to MemberRegisteredEvent.
/// Sends a welcome notification when a member joins.
/// </summary>
public class WelcomeEmailHandler : IDomainEventHandler<MemberRegisteredEvent>
{
    private readonly Action<string> _log;

    public WelcomeEmailHandler(Action<string>? log = null)
    {
        _log = log ?? Console.WriteLine;
    }

    public Task HandleAsync(MemberRegisteredEvent domainEvent)
    {
        _log($"[Email] Welcome email sent to {domainEvent.Member.Email} ({domainEvent.Member.FullName})");
        return Task.CompletedTask;
    }
}

/// <summary>
/// Observer Pattern: reacts to MemberDeactivatedEvent.
/// Logs when a member is deactivated.
/// </summary>
public class MemberDeactivatedAuditHandler : IDomainEventHandler<MemberDeactivatedEvent>
{
    private readonly Action<string> _log;

    public MemberDeactivatedAuditHandler(Action<string>? log = null)
    {
        _log = log ?? Console.WriteLine;
    }

    public Task HandleAsync(MemberDeactivatedEvent domainEvent)
    {
        _log($"[Audit] Member deactivated: {domainEvent.Member.FullName} at {domainEvent.OccurredAt:O}");
        return Task.CompletedTask;
    }
}
