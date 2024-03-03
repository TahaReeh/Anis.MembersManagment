using Anis.MembersManagment.Query.Entities;

namespace Anis.MembersManagment.Query.QueryHandlers.MemberPendingInvitations
{
    public record MemberPendingInvitationsResult(
        List<Invitation> Invitations,
        int Page,
        int PageSize,
        int TotalResults
        );
}
