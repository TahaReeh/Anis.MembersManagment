using Anis.MembersManagment.Query.Entities;

namespace Anis.MembersManagment.Query.QueryHandlers.OwnerPendingInvitations
{
    public record OwnerPendingInvitationsResult(
        List<Invitation> Invitations,
        int Page,
        int PageSize,
        int TotalResults
        );
}
