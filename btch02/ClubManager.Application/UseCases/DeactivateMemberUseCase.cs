using ClubManager.Domain.Interfaces;

namespace ClubManager.Application.UseCases;

/// <summary>
/// SRP: DeactivateMemberUseCase handles only deactivation.
/// </summary>
public class DeactivateMemberUseCase
{
    private readonly IMemberRepository _memberRepository;

    public DeactivateMemberUseCase(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task ExecuteAsync(Guid memberId)
    {
        var member = await _memberRepository.GetByIdAsync(memberId)
            ?? throw new KeyNotFoundException($"Member {memberId} not found.");

        member.Deactivate();
        await _memberRepository.UpdateAsync(member);
    }
}
