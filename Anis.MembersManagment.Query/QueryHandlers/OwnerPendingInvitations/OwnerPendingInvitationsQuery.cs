using MediatR;

namespace Anis.MembersManagment.Query.QueryHandlers.OwnerPendingInvitations
{
    public record OwnerPendingInvitationsQuery(
        string UserId,
        int Page,
        int Size
        ) : IRequest<OwnerPendingInvitationsResult>;
}
