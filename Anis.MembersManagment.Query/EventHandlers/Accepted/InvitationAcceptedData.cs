namespace Anis.MembersManagment.Query.EventHandlers.Accepted
{
    public record InvitationAcceptedData(
       string Id,
       string AccountId,
       string SubscriptionId,
       string MemberId
        );
}
