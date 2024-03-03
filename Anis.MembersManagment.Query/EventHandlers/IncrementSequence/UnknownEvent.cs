namespace Anis.MembersManagment.Query.EventHandlers.IncrementSequence
{
    public record class UnknownEvent(
        string AggregateId,
        int Sequence,
        DateTime DateTime,
        string UserId,
        int Version
        ) : Event<object>(
            AggregateId: AggregateId,
            Sequence: Sequence,
            Data: new(),
            DateTime: DateTime,
            UserId: UserId,
            Version: Version
            );
}
