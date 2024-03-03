using MediatR;

namespace Anis.MembersManagment.Query.QueryHandlers.MemberPendingInvitations
{
    public record MemberPendingInvitationsQuery(
        string UserId,
        int Page,
        int Size
        ) : IRequest<MemberPendingInvitationsResult>;
   
}
