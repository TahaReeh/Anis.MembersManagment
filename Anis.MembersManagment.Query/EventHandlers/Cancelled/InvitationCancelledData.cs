namespace Anis.MembersManagment.Query.EventHandlers.Cancelled
{
    //string user_id = 5; why ????
    public record InvitationCancelledData(
       string Id,
       string AccountId,
       string SubscriptionId,
       string MemberId
        );
}
