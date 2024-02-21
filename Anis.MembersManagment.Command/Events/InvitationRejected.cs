namespace Anis.MembersManagment.Command.Events
{
    public record InvitationRejected(
        string AggregateId,
        int Sequence,
        DateTime DateTime,
        InvitationRejectedData Data,
        string UserId,
        int Version
        ) : Event<InvitationRejectedData>(AggregateId, Sequence, DateTime, Data, UserId, Version);

    public record InvitationRejectedData(
      string AccountId,
      string SubscriptionId,
      string MemberId
      );
}