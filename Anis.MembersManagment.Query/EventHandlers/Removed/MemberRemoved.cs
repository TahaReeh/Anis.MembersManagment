namespace Anis.MembersManagment.Query.EventHandlers.Removed
{
    public record class MemberRemoved(
        string AggregateId,
        int Sequence,
        MemberRemovedData Data,
        DateTime DateTime,
        string UserId,
        int Version
        ) : Event<MemberRemovedData>(
            AggregateId: AggregateId,
            Sequence: Sequence,
            Data: Data,
            DateTime: DateTime,
            UserId: UserId,
            Version: Version
            );
}
