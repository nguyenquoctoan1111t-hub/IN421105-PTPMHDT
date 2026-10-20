using ClubManager.Domain.Interfaces;

namespace ClubManager.Application.UseCases;

/// <summary>
/// SRP: CalculateFeeUseCase only computes fee using injected strategy.
/// Strategy Pattern: delegates algorithm to IFeeCalculationStrategy.
/// </summary>
public class CalculateFeeUseCase
{
    private readonly IMemberRepository _memberRepository;
    private readonly IFeeCalculationStrategy _feeStrategy;

    public CalculateFeeUseCase(IMemberRepository memberRepository, IFeeCalculationStrategy feeStrategy)
    {
        _memberRepository = memberRepository;
        _feeStrategy = feeStrategy;
    }

    public async Task<decimal> ExecuteAsync(Guid memberId, int months)
    {
        var member = await _memberRepository.GetByIdAsync(memberId)
            ?? throw new KeyNotFoundException($"Member {memberId} not found.");

        return _feeStrategy.Calculate(member, months);
    }
}
