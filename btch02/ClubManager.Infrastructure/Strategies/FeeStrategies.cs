using ClubManager.Domain.Entities;
using ClubManager.Domain.Interfaces;

namespace ClubManager.Infrastructure.Strategies;

/// <summary>
/// Strategy Pattern - Basic fee: flat rate of 100k/month.
/// </summary>
public class BasicFeeStrategy : IFeeCalculationStrategy
{
    public string StrategyName => "Basic";
    private const decimal MonthlyRate = 100_000m;

    public decimal Calculate(Member member, int months) => MonthlyRate * months;
}

/// <summary>
/// Strategy Pattern - Premium fee: 180k/month.
/// OCP: Adding a new strategy does not modify existing strategies.
/// </summary>
public class PremiumFeeStrategy : IFeeCalculationStrategy
{
    public string StrategyName => "Premium";
    private const decimal MonthlyRate = 180_000m;

    public decimal Calculate(Member member, int months) => MonthlyRate * months;
}

/// <summary>
/// Strategy Pattern - VIP fee: 250k/month with 10% loyalty discount if >= 6 months.
/// </summary>
public class VipFeeStrategy : IFeeCalculationStrategy
{
    public string StrategyName => "VIP";
    private const decimal MonthlyRate = 250_000m;

    public decimal Calculate(Member member, int months)
    {
        var total = MonthlyRate * months;
        if (months >= 6) total *= 0.9m; // 10% loyalty discount
        return total;
    }
}
