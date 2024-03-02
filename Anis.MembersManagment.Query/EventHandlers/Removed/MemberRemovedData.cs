namespace Anis.MembersManagment.Query.EventHandlers.Removed
{
    public record MemberRemovedData(
       string Id,
       string AccountId,
       string SubscriptionId,
       string MemberId
        );
}
