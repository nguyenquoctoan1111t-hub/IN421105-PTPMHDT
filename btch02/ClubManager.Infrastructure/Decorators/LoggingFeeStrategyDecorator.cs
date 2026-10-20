using ClubManager.Domain.Entities;
using ClubManager.Domain.Interfaces;

namespace ClubManager.Infrastructure.Decorators;

/// <summary>
/// Decorator Pattern: wraps IFeeCalculationStrategy to add logging.
/// OCP: adds cross-cutting concern (logging) without modifying core strategy.
/// </summary>
public class LoggingFeeStrategyDecorator : IFeeCalculationStrategy
{
    private readonly IFeeCalculationStrategy _inner;
    private readonly Action<string> _log;

    public LoggingFeeStrategyDecorator(IFeeCalculationStrategy inner, Action<string>? log = null)
    {
        _inner = inner;
        _log = log ?? Console.WriteLine;
    }

    public string StrategyName => $"Logging({_inner.StrategyName})";

    public decimal Calculate(Member member, int months)
    {
        _log($"[Fee] Calculating fee for member '{member.FullName}' ({_inner.StrategyName}) x{months} months");
        var fee = _inner.Calculate(member, months);
        _log($"[Fee] Result: {fee:N0} VND");
        return fee;
    }
}
