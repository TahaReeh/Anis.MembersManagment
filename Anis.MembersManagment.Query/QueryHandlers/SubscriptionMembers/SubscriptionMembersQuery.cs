using MediatR;

namespace Anis.MembersManagment.Query.QueryHandlers.SubscriptionMembers
{
    public record SubscriptionMembersQuery(
        string SubscriptionId,
        int Page,
        int Size
        ) : IRequest<SubscriptionMembersResult>;
}
