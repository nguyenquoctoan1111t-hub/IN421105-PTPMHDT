using ClubManager.Domain.Entities;
using ClubManager.Domain.Interfaces;

namespace ClubManager.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of IClubRepository.
/// </summary>
public class InMemoryClubRepository : IClubRepository
{
    private readonly Dictionary<Guid, Club> _store = new();

    public Task<Club?> GetByIdAsync(Guid id)
    {
        _store.TryGetValue(id, out var club);
        return Task.FromResult(club);
    }

    public Task<IEnumerable<Club>> GetAllAsync()
        => Task.FromResult<IEnumerable<Club>>(_store.Values.ToList());

    public Task AddAsync(Club club)
    {
        _store[club.Id] = club;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Club club)
    {
        _store[club.Id] = club;
        return Task.CompletedTask;
    }
}
