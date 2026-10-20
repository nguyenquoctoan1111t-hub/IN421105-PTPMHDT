using ClubManager.Domain.Events;

namespace ClubManager.Domain.Entities;

public class Member
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public MembershipType MembershipType { get; private set; }
    public DateTime JoinedAt { get; private set; }
    public bool IsActive { get; private set; }

    // Observer pattern: collect domain events
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Member() { } // for ORM

    public Member(string fullName, string email, MembershipType membershipType)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("FullName is required.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        MembershipType = membershipType;
        JoinedAt = DateTime.UtcNow;
        IsActive = true;

        _domainEvents.Add(new MemberRegisteredEvent(this));
    }

    public void Deactivate()
    {
        if (!IsActive) throw new InvalidOperationException("Member is already inactive.");
        IsActive = false;
        _domainEvents.Add(new MemberDeactivatedEvent(this));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
