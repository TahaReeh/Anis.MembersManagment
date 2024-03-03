using Anis.MembersManagment.Query.Entities;

namespace Anis.MembersManagment.Query.QueryHandlers.MemberSubscriptions
{
    public record MemberSubscriptionsResult(
        List<Subscriber> Subscribers,
        int Page,
        int PageSize,
        int TotalResults);
}
