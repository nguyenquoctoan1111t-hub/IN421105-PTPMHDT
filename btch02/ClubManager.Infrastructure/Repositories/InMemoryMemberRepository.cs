using ClubManager.Domain.Entities;
using ClubManager.Domain.Interfaces;

namespace ClubManager.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of IMemberRepository.
/// DIP: Application depends on abstraction; Infrastructure provides concrete implementation.
/// </summary>
public class InMemoryMemberRepository : IMemberRepository
{
    private readonly Dictionary<Guid, Member> _store = new();

    public Task<Member?> GetByIdAsync(Guid id)
    {
        _store.TryGetValue(id, out var member);
        return Task.FromResult(member);
    }

    public Task<IEnumerable<Member>> GetAllAsync()
        => Task.FromResult<IEnumerable<Member>>(_store.Values.ToList());

    public Task AddAsync(Member member)
    {
        _store[member.Id] = member;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Member member)
    {
        _store[member.Id] = member;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _store.Remove(id);
        return Task.CompletedTask;
    }
}
