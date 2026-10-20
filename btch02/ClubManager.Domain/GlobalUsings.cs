using ClubManager.Domain.Events;

namespace ClubManager.Domain.Entities;

// Fix: Member.cs references IDomainEvent from Events namespace
// This partial bridge ensures the using works correctly via global using
