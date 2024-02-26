namespace Anis.MembersManagment.Command.Events
{
    public record MemberRemoved(
        string AggregateId,
        int Sequence,
        DateTime DateTime,
        MemberRemovedData Data,
        string UserId,
        int Version
        ) : Event<MemberRemovedData>(AggregateId, Sequence, DateTime, Data, UserId, Version);

    public record MemberRemovedData(
        string AccountId,
        string SubscriptionId,
        string MemberId
  );
}
