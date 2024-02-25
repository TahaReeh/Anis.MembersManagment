using Anis.MembersManagment.Command.Domain;

namespace Anis.MembersManagment.Command.Events
{
    public record MemberJoined(
        string AggregateId,
        int Sequence,
        DateTime DateTime,
        MemberJoinedData Data,
        string UserId,
        int Version
        ) : Event<MemberJoinedData>(AggregateId, Sequence, DateTime, Data, UserId, Version);

    public record MemberJoinedData(
       string AccountId,
       string SubscriptionId,
       string MemberId,
       Permission Permissions
       );
}
