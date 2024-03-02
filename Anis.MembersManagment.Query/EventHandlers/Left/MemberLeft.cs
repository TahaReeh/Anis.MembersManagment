namespace Anis.MembersManagment.Query.EventHandlers.Left
{
    public record class MemberLeft(
        string AggregateId,
        int Sequence,
        MemberLeftData Data,
        DateTime DateTime,
        string UserId,
        int Version
        ) : Event<MemberLeftData>(
            AggregateId: AggregateId,
            Sequence: Sequence,
            Data: Data,
            DateTime: DateTime,
            UserId: UserId,
            Version: Version
            );
}
