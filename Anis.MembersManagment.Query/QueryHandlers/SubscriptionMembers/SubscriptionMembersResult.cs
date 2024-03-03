using Anis.MembersManagment.Query.Entities;

namespace Anis.MembersManagment.Query.QueryHandlers.SubscriptionMembers
{
    public record SubscriptionMembersResult(
        List<Subscriber> Subscribers,
        int Page,
        int PageSize,
        int TotalResults
        );
}
