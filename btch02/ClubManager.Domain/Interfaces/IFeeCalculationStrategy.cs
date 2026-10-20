using ClubManager.Domain.Entities;

namespace ClubManager.Domain.Interfaces;

/// <summary>
/// Strategy Pattern: defines interchangeable fee calculation algorithms.
/// OCP: new fee strategies can be added without modifying existing code.
/// </summary>
public interface IFeeCalculationStrategy
{
    decimal Calculate(Member member, int months);
    string StrategyName { get; }
}
