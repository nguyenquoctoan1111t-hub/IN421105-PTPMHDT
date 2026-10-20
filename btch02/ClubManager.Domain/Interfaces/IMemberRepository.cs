using ClubManager.Domain.Entities;

namespace ClubManager.Domain.Interfaces;

/// <summary>
/// ISP: Separated read and write responsibilities for member repository.
/// DIP: Application depends on this abstraction, not concrete implementations.
/// </summary>
public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id);
    Task<IEnumerable<Member>> GetAllAsync();
    Task AddAsync(Member member);
    Task UpdateAsync(Member member);
    Task DeleteAsync(Guid id);
}
