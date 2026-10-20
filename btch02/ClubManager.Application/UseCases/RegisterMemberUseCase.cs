using ClubManager.Domain.Entities;
using ClubManager.Domain.Interfaces;

namespace ClubManager.Application.UseCases;

/// <summary>
/// SRP: RegisterMemberUseCase only handles member registration.
/// DIP: depends on IMemberRepository abstraction.
/// </summary>
public class RegisterMemberUseCase
{
    private readonly IMemberRepository _memberRepository;

    public RegisterMemberUseCase(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Member> ExecuteAsync(string fullName, string email, MembershipType membershipType)
    {
        var member = new Member(fullName, email, membershipType);
        await _memberRepository.AddAsync(member);
        return member;
    }
}
