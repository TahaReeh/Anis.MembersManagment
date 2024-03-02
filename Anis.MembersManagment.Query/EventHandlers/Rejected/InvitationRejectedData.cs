namespace Anis.MembersManagment.Query.EventHandlers.Rejected
{
    public record  InvitationRejectedData(
       string Id,
       string AccountId,
       string SubscriptionId,
       string MemberId
        );
}
