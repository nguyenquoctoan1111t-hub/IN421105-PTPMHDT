using ClubManager.Application.UseCases;
using ClubManager.Domain.Entities;
using ClubManager.Infrastructure.Decorators;
using ClubManager.Infrastructure.EventHandlers;
using ClubManager.Infrastructure.Factories;
using ClubManager.Infrastructure.Repositories;

Console.WriteLine("=== ClubManager SOLID Demo ===\n");

// DIP: inject concrete implementations via interfaces
var memberRepo = new InMemoryMemberRepository();
var clubRepo   = new InMemoryClubRepository();

// Factory Pattern: create strategy based on membership type
var strategy = FeeStrategyFactory.Create(MembershipType.VIP);

// Decorator Pattern: wrap strategy with logging
var decoratedStrategy = new LoggingFeeStrategyDecorator(strategy);

// Use cases (SRP: each use case has one responsibility)
var registerUseCase   = new RegisterMemberUseCase(memberRepo);
var feeUseCase        = new CalculateFeeUseCase(memberRepo, decoratedStrategy);
var deactivateUseCase = new DeactivateMemberUseCase(memberRepo);

// Observer Pattern: event handlers
var welcomeHandler  = new WelcomeEmailHandler();
var auditHandler    = new MemberDeactivatedAuditHandler();

// Register a member
var member = await registerUseCase.ExecuteAsync("Nguyen Van A", "nva@example.com", MembershipType.VIP);

// Dispatch domain events (Observer pattern)
foreach (var @event in member.DomainEvents)
{
    if (@event is ClubManager.Domain.Events.MemberRegisteredEvent re)
        await welcomeHandler.HandleAsync(re);
}
member.ClearDomainEvents();

Console.WriteLine($"\n✓ Registered: {member.FullName} [{member.MembershipType}] Id={member.Id}\n");

// Calculate fee using Strategy + Decorator
var fee = await feeUseCase.ExecuteAsync(member.Id, 6);
Console.WriteLine($"\n✓ Total fee for 6 months: {fee:N0} VND\n");

// Deactivate member
await deactivateUseCase.ExecuteAsync(member.Id);
var updatedMember = await memberRepo.GetByIdAsync(member.Id);

// Dispatch deactivation events
foreach (var @event in updatedMember!.DomainEvents)
{
    if (@event is ClubManager.Domain.Events.MemberDeactivatedEvent de)
        await auditHandler.HandleAsync(de);
}

Console.WriteLine($"\n✓ Member deactivated. IsActive={updatedMember.IsActive}");
Console.WriteLine("\n=== Done ===");
