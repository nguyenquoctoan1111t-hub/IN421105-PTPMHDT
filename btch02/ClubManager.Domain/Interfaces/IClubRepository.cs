using ClubManager.Domain.Entities;

namespace ClubManager.Domain.Interfaces;

/// <summary>
/// DIP: Club use-cases depend on abstraction, not concrete storage.
/// </summary>
public interface IClubRepository
{
    Task<Club?> GetByIdAsync(Guid id);
    Task<IEnumerable<Club>> GetAllAsync();
    Task AddAsync(Club club);
    Task UpdateAsync(Club club);
}
