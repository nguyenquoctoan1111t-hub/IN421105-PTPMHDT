using ClubManager.Application.UseCases;
using ClubManager.Domain.Entities;
using ClubManager.Domain.Events;
using ClubManager.Infrastructure.Decorators;
using ClubManager.Infrastructure.Factories;
using ClubManager.Infrastructure.Repositories;
using ClubManager.Infrastructure.Strategies;

namespace ClubManager.Tests;

public class MemberTests
{
    // Test 1: Register a member successfully
    [Fact]
    public void Member_WhenCreated_ShouldBeActiveAndHaveRegisteredEvent()
    {
        // Arrange
        var fullName = "Tran Thi B";
        var email = "ttb@example.com";
        var membershipType = MembershipType.Premium;

        // Act
        var member = new Member(fullName, email, membershipType);

        // Assert
        Assert.True(member.IsActive);
        Assert.Equal(fullName, member.FullName);
        Assert.Equal(email, member.Email);
        Assert.Equal(membershipType, member.MembershipType);
        Assert.Single(member.DomainEvents);
        Assert.IsType<MemberRegisteredEvent>(member.DomainEvents[0]);
    }

    // Test 2: Deactivate a member raises domain event
    [Fact]
    public void Member_WhenDeactivated_ShouldBeInactiveAndRaiseEvent()
    {
        // Arrange
        var member = new Member("Le Van C", "lvc@example.com", MembershipType.Basic);
        member.ClearDomainEvents();

        // Act
        member.Deactivate();

        // Assert
        Assert.False(member.IsActive);
        Assert.Single(member.DomainEvents);
        Assert.IsType<MemberDeactivatedEvent>(member.DomainEvents[0]);
    }

    // Test 3: Cannot deactivate already-inactive member
    [Fact]
    public void Member_WhenDeactivatedTwice_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var member = new Member("Pham Thi D", "ptd@example.com", MembershipType.VIP);
        member.Deactivate();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => member.Deactivate());
    }

    // Test 4: Member creation with empty name throws exception
    [Fact]
    public void Member_WhenCreatedWithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Member("", "test@example.com", MembershipType.Basic));
    }
}

public class FeeStrategyTests
{
    // Test 5: VIP fee with 6+ months gets 10% discount
    [Fact]
    public void VipFeeStrategy_WhenMonthsGreaterOrEqual6_ShouldApply10PercentDiscount()
    {
        // Arrange
        var strategy = new VipFeeStrategy();
        var member = new Member("VIP User", "vip@example.com", MembershipType.VIP);
        var months = 6;

        // Act
        var fee = strategy.Calculate(member, months);

        // Assert
        var expectedFee = 250_000m * 6 * 0.9m; // 1,350,000
        Assert.Equal(expectedFee, fee);
    }

    // Test 6: FeeStrategyFactory creates correct strategy by membership type
    [Fact]
    public void FeeStrategyFactory_WhenGivenPremiumType_ShouldReturnPremiumStrategy()
    {
        // Arrange
        var membershipType = MembershipType.Premium;

        // Act
        var strategy = FeeStrategyFactory.Create(membershipType);

        // Assert
        Assert.IsType<PremiumFeeStrategy>(strategy);
        Assert.Equal("Premium", strategy.StrategyName);
    }
}

public class RegisterMemberUseCaseTests
{
    // Test 7: RegisterMemberUseCase stores member in repo
    [Fact]
    public async Task RegisterMemberUseCase_WhenExecuted_ShouldPersistMemberInRepository()
    {
        // Arrange
        var repo = new InMemoryMemberRepository();
        var useCase = new RegisterMemberUseCase(repo);

        // Act
        var member = await useCase.ExecuteAsync("Nguyen Van E", "nve@example.com", MembershipType.Basic);

        // Assert
        var stored = await repo.GetByIdAsync(member.Id);
        Assert.NotNull(stored);
        Assert.Equal("Nguyen Van E", stored!.FullName);
    }
}
