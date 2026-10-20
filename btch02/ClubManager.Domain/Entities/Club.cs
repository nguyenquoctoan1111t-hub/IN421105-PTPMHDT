namespace ClubManager.Domain.Entities;

public class Club
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int MaxCapacity { get; private set; }
    private readonly List<Guid> _memberIds = new();
    public IReadOnlyList<Guid> MemberIds => _memberIds.AsReadOnly();

    private Club() { }

    public Club(string name, string description, int maxCapacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Club name is required.", nameof(name));
        if (maxCapacity <= 0)
            throw new ArgumentException("MaxCapacity must be positive.", nameof(maxCapacity));

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        MaxCapacity = maxCapacity;
    }

    public void AddMember(Guid memberId)
    {
        if (_memberIds.Count >= MaxCapacity)
            throw new InvalidOperationException("Club has reached maximum capacity.");
        if (_memberIds.Contains(memberId))
            throw new InvalidOperationException("Member already belongs to this club.");
        _memberIds.Add(memberId);
    }

    public void RemoveMember(Guid memberId)
    {
        if (!_memberIds.Remove(memberId))
            throw new InvalidOperationException("Member not found in this club.");
    }

    public bool IsFull => _memberIds.Count >= MaxCapacity;
}
