namespace Anis.MembersManagment.Command.Events
{
    public record MemberLeft(
        string AggregateId,
        int Sequence,
        DateTime DateTime,
        MemberLeftData Data,
        string UserId,
        int Version
        ) :Event<MemberLeftData>(AggregateId, Sequence, DateTime, Data, UserId, Version);

    public record MemberLeftData(
         string AccountId,
        string SubscriptionId,
        string MemberId
        );
}
