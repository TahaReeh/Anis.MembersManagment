namespace Anis.MembersManagment.Command.Events
{
    public record InvitationCancelled(
        string AggregateId,
        int Sequence,
        DateTime DateTime,
        InvitationCancelledData Data,
        string UserId,
        int Version
        ) : Event<InvitationCancelledData>(AggregateId, Sequence, DateTime, Data, UserId, Version);

    public record InvitationCancelledData(
      string AccountId,
      string SubscriptionId,
      string MemberId
      );
}