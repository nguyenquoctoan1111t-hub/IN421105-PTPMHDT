using ClubManager.Domain.Entities;
using ClubManager.Domain.Interfaces;
using ClubManager.Infrastructure.Strategies;

namespace ClubManager.Infrastructure.Factories;

/// <summary>
/// Factory Pattern: creates the correct IFeeCalculationStrategy based on MembershipType.
/// OCP: new membership types can be added by extending, not modifying this factory.
/// </summary>
public static class FeeStrategyFactory
{
    public static IFeeCalculationStrategy Create(MembershipType membershipType)
        => membershipType switch
        {
            MembershipType.Basic   => new BasicFeeStrategy(),
            MembershipType.Premium => new PremiumFeeStrategy(),
            MembershipType.VIP     => new VipFeeStrategy(),
            _ => throw new NotSupportedException($"No fee strategy for {membershipType}")
        };
}
